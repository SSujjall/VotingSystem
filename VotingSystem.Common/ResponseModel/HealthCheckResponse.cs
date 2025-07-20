using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingSystem.Common.ResponseModel
{
    public class HealthCheckResponse
    {
        public string OverallStatus { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public List<HealthCheckItem> Checks { get; set; }
    }

    public class HealthCheckItem
    {
        public string Component { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }
}
