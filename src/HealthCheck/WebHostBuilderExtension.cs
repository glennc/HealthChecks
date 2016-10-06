using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HealthCheck;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostBuilderExtension
    {
        public static IWebHostBuilder UseHealthChecks(this IWebHostBuilder builder, int port)
        {
            //Add a single check that always passes.
            //TODO: This probably isn't required.
            builder.UseHealthChecks(port, checks => {
                checks.AddPassingCheck();
            });
            return builder;
        }

        public static IWebHostBuilder UseHealthChecks(this IWebHostBuilder builder, int port, Action<HealthCheckBuilder> checkupAction)
        {
            var checkupService = new HealthCheckService();
            var checkupBuilder = new HealthCheckBuilder(checkupService);

            checkupAction.Invoke(checkupBuilder);

            builder.ConfigureServices(services => {
                var existingUrl = builder.GetSetting(WebHostDefaults.ServerUrlsKey);
                builder.UseSetting(WebHostDefaults.ServerUrlsKey, $"{existingUrl};http://localhost:{port}");

                //TODO: This is not likely to be how we would flow settings...
                services.AddSingleton<IHealthCheckService>(checkupService);
                services.AddSingleton<HealthCheckOptions>(new HealthCheckOptions{ HealthCheckPort = port});
                services.AddSingleton<IStartupFilter>(new HealthCheckStartupFilter(port));
            });
            return builder;
        }
    }
}
