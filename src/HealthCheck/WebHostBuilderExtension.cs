using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HealthCheck;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostBuilderExtension
    {
        public static IWebHostBuilder UseHealthChecks(this IWebHostBuilder builder, int port, Action<HealthCheckBuilder> checkupAction)
        {
            var checkupBuilder = new HealthCheckBuilder();

            checkupAction.Invoke(checkupBuilder);

            builder.ConfigureServices(services => {
                var existingUrl = builder.GetSetting(WebHostDefaults.ServerUrlsKey);
                builder.UseSetting(WebHostDefaults.ServerUrlsKey, $"{existingUrl};http://localhost:{port}");

                services.AddSingleton(checkupBuilder);
                services.AddSingleton(new HealthCheckOptions{ HealthCheckPort = port});
                services.AddSingleton<IHealthCheckService, HealthCheckService>();
                services.AddSingleton<IStartupFilter>(new HealthCheckStartupFilter(port));
            });
            return builder;
        }
    }
}
