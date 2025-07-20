using Microsoft.Extensions.Diagnostics.HealthChecks;
using VotingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace VotingSystem.Infrastructure.Health
{
    public sealed class DbHealthCheck : IHealthCheck
    {
        private readonly AppDbContext _dbContext;

        public DbHealthCheck(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync("Select 1;", cancellationToken);
                return HealthCheckResult.Healthy("Database is reachable");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database is not reachable", exception: ex);
            }
        }
    }
}
