using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;

#if XAMLFORBLAZOR
namespace XamlForBlazor
#else
namespace OpenSilver.Compatibility.Blazor
#endif
{
    /// <summary>
    /// The Initializer class is a static class that contains methods for initializing the application.
    /// </summary>
    public static class Initializer
    {
        internal static bool Initialized = false;

        private const string RazorComponentInstantiatorTag = "razorcomponentinstantiator-tag";

        /// <summary>
        /// Initializes the application by registering a custom element.
        /// Must be called from the Program.Main method.
        /// </summary>
        /// <param name="builder">The WebAssemblyHostBuilder used to configure the application's services and start the application.</param>
        public static void Initialize(WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.RegisterCustomElement<RazorComponentInstantiator>(RazorComponentInstantiatorTag);
            Initialized = true;
        }
    }
}
