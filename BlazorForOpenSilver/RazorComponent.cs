

/*===================================================================================
* 
*   Copyright (c) Userware/OpenSilver.net
*
*   This file is part of the OpenSilver Runtime (https://opensilver.net), which is
*   licensed under the MIT license: https://opensource.org/licenses/MIT
*
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*
\*====================================================================================*/

using System;
using System.Collections.Generic;
using System.Threading;
using CSHTML5.Native.Html.Controls;
using System.Windows;
using Microsoft.AspNetCore.Components.RenderTree;

#if XAMLFORBLAZOR
namespace XamlForBlazor
#else
namespace OpenSilver.Compatibility.Blazor
#endif
{
    public class RazorComponent : HtmlPresenter
    {
        private static Dictionary<int, WeakReference<RazorComponent>> _beNotifiedWhenRazorComponentInstantiatorRendered = new Dictionary<int, WeakReference<RazorComponent>>();
        private static int m_Counter = 0; // This is to generate unique identifiers that are incremented for each instance.

        private RazorComponentInstantiator? _razorComponentInstantiator;
        private readonly int _uniqueId;

        public event RoutedEventHandler OnComponentRendered;

        public bool Rendered;

        static RazorComponent()
        {
            if (!Initializer.Initialized)
            {
                var line = "OpenSilver.Compatibility.Blazor.Initializer.Initialize(builder);";
#if XAMLFORBLAZOR
                line = "XamlForBlazor.Initializer.Initialize(builder);";
#endif
                MessageBox.Show(
                    $"The application has not been initialized properly. Please add the line '{line}' to the Program.Main method before the builder.build() command.");
            }
        }

        public RazorComponent()
        {
            //todo: maybe use "Guid" instead of "int" when the compiler error "Could not load file or assembly 'System.Runtime, Version=7.0.0.0" is fixed?
            _uniqueId = Interlocked.Increment(ref m_Counter);

            this.Html = $"<razorcomponentinstantiator-tag uniqueid=\"{_uniqueId}\"></razorcomponentinstantiator-tag>";

            _beNotifiedWhenRazorComponentInstantiatorRendered.Add(_uniqueId, new WeakReference<RazorComponent>(this));
        }

        public void OnRazorComponentInstantiatorRendered(RazorComponentInstantiator razorComponentInstantiator)
        {
            _razorComponentInstantiator = razorComponentInstantiator;
            _razorComponentInstantiator.ComponentType = this.ComponentType;
        }

        public static void RaiseOnRazorComponentInstantiatorRendered(int uniqueId, RazorComponentInstantiator razorComponentInstantiator, bool bFirstRender)
        {
            if (_beNotifiedWhenRazorComponentInstantiatorRendered.TryGetValue(uniqueId, out WeakReference<RazorComponent>? weakRef))
            {
                if (weakRef.TryGetTarget(out RazorComponent? razorComponent))
                {
                    razorComponent.OnRazorComponentInstantiatorRendered(razorComponentInstantiator);

                    if (!bFirstRender)
                    {
                        razorComponent.Rendered = true;
                        razorComponent.OnComponentRendered?.Invoke(razorComponent, new RoutedEventArgs());
                    }
                }
            }
            if (!bFirstRender)
            {
                _beNotifiedWhenRazorComponentInstantiatorRendered.Remove(uniqueId); //Note: deliberately fails silently if not found.
            }
        }

        private Type? _componentType;
        public Type? ComponentType
        {
            get { return _componentType; }
            set { _componentType = value; }
        }

        public object? Instance
        {
            get
            {
                return _razorComponentInstantiator?.ComponentInstance;
            }
        }
    }
}
