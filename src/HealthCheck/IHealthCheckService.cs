using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCheck
{
    public interface IHealthCheckService
    {
        Task<bool> CheckHealthAsync();
    }
}