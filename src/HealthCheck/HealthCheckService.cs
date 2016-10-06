using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HealthCheck
{
    public class HealthCheckService : IHealthCheckService
    {
        public List<Func<bool>> Checks { get; set; } = new List<Func<bool>>();

        //TODO: Decide which of these I like better.
        public async Task<bool> CheckHealthAsync()
        {
            var checkTasks = new Task<bool>[Checks.Count];
            for(int i = 0; i < Checks.Count; i++)
            {
                checkTasks[i] = Task.Run(() => {
                    try
                    {
                        return Checks[i].Invoke();
                    }
                    catch
                    {
                        return false;
                    }
                });
            }

            await Task.WhenAll(checkTasks);

            return !checkTasks.Any(x=>!x.Result);
        }

        public bool CheckHealth()
        {
            var healthy = true;
            Parallel.ForEach(Checks, check => {
                try
                {
                    var result = check.Invoke();
                    healthy &= result;
                }
                catch
                {
                    healthy &= false;
                }
            });
            return healthy;
        }
    } 
}
