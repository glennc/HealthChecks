using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCheck.Checks
{
    public class CustomCheck : IHealthCheck
    {
        public string Name
        {
            get
            {
                return "CustomCheck";
            }
        }

        public bool Execute()
        {
            return false;
        }
    }
}
