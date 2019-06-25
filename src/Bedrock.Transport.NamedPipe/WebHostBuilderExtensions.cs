using Bedrock.Transport.NamedPipe;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseNamedPipe(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IConnectionListenerFactory, NamedPipeConnectionListenerFactory>();
            });
        }

        public static IWebHostBuilder UseNamedPipe(this IWebHostBuilder hostBuilder, Action<NamedPipeTransportOptions> configureOptions)
        {
            return hostBuilder.UseNamedPipe().ConfigureServices(services =>
            {
                services.Configure(configureOptions);
            });
        }
    }
}
