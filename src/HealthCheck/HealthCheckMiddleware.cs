using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace HealthCheck
{
    public class HealthCheckMiddleware
    {
        RequestDelegate _next;
        int _healthCheckPort;
        IHealthCheckService _checkupService;

        public HealthCheckMiddleware(RequestDelegate next, HealthCheckOptions options, IHealthCheckService checkupService)
        {
            _healthCheckPort = options.HealthCheckPort;
            _checkupService = checkupService;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var connInfo = context.Features.Get<IHttpConnectionFeature>();
            if(connInfo.LocalPort == _healthCheckPort)
            {
                var healthy = _checkupService.CheckHealth();
                if(healthy)
                {
                    await context.Response.WriteAsync("HealthCheck: OK");
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("HealthStatus: Unhealthy");
                }
                return;
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}