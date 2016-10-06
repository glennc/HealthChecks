using System;
using HealthCheck;
using Microsoft.AspNetCore.Hosting;

namespace Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                            .UseKestrel()
                            .UseStartup<Startup>()
                            .UseUrls("http://+:5000")
                            .UseHealthChecks(8080, checks =>
                            {
                                checks.AddUrlCheck("http://google.com")
                                      .AddCustomCheck(CheckAppReady)
                                      .AddFailingCheck();
                            })
                            .Build();

            var healthChecks = host.Services.GetService(typeof(IHealthCheckService)) as IHealthCheckService;

            var healthy = healthChecks.CheckHealth();
            Console.WriteLine(healthy ? "Application is Healthy" : "Application is not healthy");

            host.RunWhenHealthy();
            //host.Run();
        }

        public static bool CheckAppReady()
        {
            return true;
        }
    }
}
