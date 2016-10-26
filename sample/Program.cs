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
                                checks.AddUrlCheck("http://gooadfgasdasle.com", (response) =>
                                {
                                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        //TODO: Some other check because I have a transparent proxy that is in-between me and the site
                                        //so I always get 200. For example.
                                        return true;
                                    }
                                    return false;
                                })
                                .AddCheck("CheckAppReady", CheckAppReady);
                            })
                            .Build();

            var healthChecks = host.Services.GetService(typeof(IHealthCheckService)) as IHealthCheckService;

            var healthy = healthChecks.CheckHealth();
            Console.WriteLine(healthy ? "Application is Healthy" : "Application is not healthy");

            host.RunWhenHealthy();
        }

        public static bool CheckAppReady()
        {
            return true;
        }
    }
}
