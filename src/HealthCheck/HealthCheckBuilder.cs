using System;
using System.Net;
using System.Net.Http;
using HealthCheck;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace HealthCheck
{
    public class HealthCheckBuilder
    {
        public Dictionary<string, Func<bool>> Checks { get; private set; }

        public HealthCheckBuilder()
        {
            Checks = new Dictionary<string, Func<bool>>();
        }

        public HealthCheckBuilder AddUrlCheck(string url)
        {
            Checks.Add($"UrlCheck ({url})", () => {
                var httpClient = new HttpClient();
                var response = httpClient.GetAsync(url).Result;
                return response.StatusCode == HttpStatusCode.OK;
            });
            return this;
        }

        public HealthCheckBuilder AddUrlCheck(string url, Func<HttpResponseMessage, bool> checkFunc)
        {
            Checks.Add($"UrlCheck ({url})", () =>{
                var httpClient = new HttpClient();
                var response = httpClient.GetAsync(url).Result;
                return checkFunc.Invoke(response);
            });
            return this;
        }

        public HealthCheckBuilder AddSqlCheck(string connectionString)
        {
            Checks.Add($"SQL Check ({connectionString})", ()=>{
                return true;
            });
            return this;
        }

        public HealthCheckBuilder AddCheck(string name, Func<bool> check)
        {
            Checks.Add(name, check);
            return this;
        }
    }
}