using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Syncfusion.Blazor;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace BlazorApp.Browser
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSyncfusionBlazor();     // for Syncfusion.Blazor components
            builder.Services.AddMudServices();          // for MudBlazor components

            // Added:
            OpenSilver.Compatibility.Blazor.Initializer.Initialize(builder);

            var host = builder.Build();
            await host.RunAsync();
        }

        public static void RunApplication()
        {
            Application.RunApplication(() =>
            {
                var app = new BlazorApp.App();
            });
        }
    }
}
