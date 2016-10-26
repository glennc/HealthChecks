using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;

namespace HealthCheck
{
    public class HealthCheckService : IHealthCheckService
    {
        public Dictionary<string, Func<bool>> _checks;
        private ILogger<HealthCheckService> _logger;

        public HealthCheckService(HealthCheckBuilder builder, ILogger<HealthCheckService> logger)
        {
            _checks = builder.Checks;
            _logger = logger;
        }

        //TODO: Decide which of these I like better.
        public async Task<bool> CheckHealthAsync()
        {
            var checkTasks = new List<Task<bool>>(_checks.Count);
            foreach(var check in _checks)
            {
                checkTasks.Add(Task.Run(() => {
                    try
                    {
                        var healthy = check.Value.Invoke();
                        _logger.LogInformation($"HealthCheck: {check.Key} : {healthy}");
                        return healthy;
                    }
                    catch
                    {
                        return false;
                    }
                }));
            }

            await Task.WhenAll(checkTasks);

            return checkTasks.All(x=>x.Result);
        }

        public bool CheckHealth()
        {
            StringBuilder logMessage = new StringBuilder();
            var healthy = true;
            Parallel.ForEach(_checks, check => {
                try
                {
                    var result = check.Value.Invoke();
                    logMessage.AppendLine($"HealthCheck: {check.Key} : {(result ? "Healthy" : "Unhealthy")}");
                    healthy &= result;
                }
                catch
                {
                    healthy &= false;
                }
            });

            _logger.Log((healthy ? LogLevel.Information : LogLevel.Error), 0, logMessage, null, MessageFormatter);

            return healthy;
        }

        private static string MessageFormatter(object state, Exception error)
        {
            return state.ToString();
        }
    } 
}
