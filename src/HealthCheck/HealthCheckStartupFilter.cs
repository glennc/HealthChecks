using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HealthCheck
{
    public class HealthCheckStartupFilter : IStartupFilter
    {
        int _port;
        public HealthCheckStartupFilter(int port)
        {
            _port = port;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app => {
                app.UseMiddleware<HealthCheckMiddleware>();
                next(app);
            };
        }
    }
}