using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HealthCheck;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostExtensions
    {
        public static void RunWhenHealthy(this IWebHost webHost)
        {
            var healthChecks = webHost.Services.GetService(typeof(IHealthCheckService)) as IHealthCheckService;

            var loops = 0;
            do
            {
                if(healthChecks.CheckHealth())
                {
                    webHost.Run();
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            } while(loops < 300);
        }
    }
}
