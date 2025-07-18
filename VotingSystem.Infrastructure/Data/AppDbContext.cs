using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Data.Seeders;

namespace VotingSystem.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //// Seeding Users
            //DbSeeder.SeedUsersToDb(builder);

            //// Seeding Entity Configurations
            //DbSeeder.SeedConfigurationToDB(builder);
        }
    }
}
