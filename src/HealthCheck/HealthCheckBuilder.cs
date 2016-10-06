using System;
using System.Net;
using System.Net.Http;
using HealthCheck;

namespace Microsoft.AspNetCore.Hosting
{
    public class HealthCheckBuilder
    {
        IHealthCheckService _service;
        
        public HealthCheckBuilder(IHealthCheckService checkupService)
        {
            _service = checkupService;
        }

        public HealthCheckBuilder AddPassingCheck()
        {
            _service.Checks.Add(() => {
                return true;
            });
            return this;
        }

        public HealthCheckBuilder AddFailingCheck()
        {
            _service.Checks.Add(() => {
                return false;
            });
            return this;
        }

        public HealthCheckBuilder AddUrlCheck(string url)
        {
            _service.Checks.Add(() =>{
                var httpClient = new HttpClient();
                var response = httpClient.GetAsync(url).Result;
                return response.StatusCode == HttpStatusCode.OK;
            });
            return this;
        }

        public HealthCheckBuilder AddSqlCheck(string connectionString)
        {
            _service.Checks.Add(()=>{
                return true;
            });
            return this;
        }

        public HealthCheckBuilder AddCustomCheck(Func<bool> customFunction)
        {
            _service.Checks.Add(() => {
                return customFunction.Invoke();
            });
            return this;
        }
    }
}