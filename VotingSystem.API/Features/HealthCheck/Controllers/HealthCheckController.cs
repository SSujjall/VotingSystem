using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VotingSystem.Common.ResponseModel;

namespace VotingSystem.API.Features.HealthCheck.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("health")]
        public async Task<IActionResult> GetHealthStatus(CancellationToken cancellationToken)
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);

            var response = new HealthCheckResponse
            {
                OverallStatus = healthReport.Status.ToString(),
                Message = healthReport.Status == HealthStatus.Healthy ? "All systems are healthy" : "One or more systems are unhealthy",
                Timestamp = DateTime.UtcNow,
                Checks = healthReport.Entries.Select(entry => new HealthCheckItem
                {
                    Component = entry.Key,
                    Status = entry.Value.Status.ToString(),
                    Description = entry.Value.Description ?? entry.Value.Exception?.Message
                }).ToList()
            };

            return Ok(response);
        }
    }
}
