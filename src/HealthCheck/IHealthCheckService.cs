using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCheck
{
    public interface IHealthCheckService
    {
        List<Func<bool>> Checks { get; set; }
        Task<bool> CheckHealthAsync();
        bool CheckHealth();
    }
}