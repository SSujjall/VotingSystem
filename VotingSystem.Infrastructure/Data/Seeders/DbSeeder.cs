using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Domain.Entities;
using VotingSystem.Domain.Enums;

namespace VotingSystem.Infrastructure.Data.Seeders
{
    public class DbSeeder
    {
        public static void SeedUsersToDb(ModelBuilder builder)
        {
            var superadminUserId = Guid.NewGuid().ToString();
            var superadminRoleId = Guid.NewGuid().ToString();
            var adminRoleId = Guid.NewGuid().ToString();

            // seed superadmin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = UserRoles.Superadmin.ToString(),
                NormalizedName = UserRoles.Superadmin.ToString().ToUpper(),
                Id = superadminRoleId,
                ConcurrencyStamp = superadminRoleId
            });

            // seed admin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = UserRoles.Admin.ToString(),
                NormalizedName = UserRoles.Admin.ToString().ToUpper(),
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            });

            // create new superadmin 
            var adminUser = new User
            {
                Id = superadminUserId,
                FullName = "Superadmin",
                Email = "superadmin@voting.com",
                UserName = "superadmin",
                NormalizedEmail = "SUPERADMIN@VOTING.COM",
                NormalizedUserName = "SUPERADMIN",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            var passwordHash = new PasswordHasher<User>();
            const string password = "Superadmin@123";

            adminUser.PasswordHash = passwordHash.HashPassword(adminUser, password);

            builder.Entity<User>().HasData(adminUser);

            // set the superadmin role to the superadmin user
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = superadminRoleId,
                UserId = superadminUserId
            });
        }

        public static void SeedConfigurationToDB(ModelBuilder builder)
        {
            builder.Entity<Poll>()
                .HasKey(p => p.PollId);

            builder.Entity<PollOption>()
                .HasKey(po => po.PollOptionId);

            builder.Entity<Vote>()
                .HasKey(v => v.VoteId);

            // Configure unique constraint: one vote per user per poll
            builder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.PollId })
                .IsUnique()
                .HasDatabaseName("IX_Votes_UserId_PollId");

            // Configure foreign key relationships with proper cascade behavior

            // Poll -> ApplicationUser (Creator)
            builder.Entity<Poll>()
                .HasOne(p => p.User)
                .WithMany(u => u.CreatedPolls)
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict); // Don't cascade delete polls when user is deleted

            // PollOption -> Poll
            builder.Entity<PollOption>()
                .HasOne(po => po.Poll)
                .WithMany(p => p.Options)
                .HasForeignKey(po => po.PollId)
                .OnDelete(DeleteBehavior.Cascade); // Delete options when poll is deleted

            // Vote -> ApplicationUser
            builder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete votes when user is deleted

            // Vote -> Poll (NO ACTION to prevent cascade cycle)
            builder.Entity<Vote>()
                .HasOne(v => v.Poll)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.PollId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade cycle

            // Vote -> PollOption (NO ACTION to prevent cascade cycle)
            builder.Entity<Vote>()
                .HasOne(v => v.PollOption)
                .WithMany(po => po.Votes)
                .HasForeignKey(v => v.PollOptionId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascade cycle

            // Configure indexes for performance
            builder.Entity<Vote>()
                .HasIndex(v => v.PollId)
                .HasDatabaseName("IX_Votes_PollId");

            builder.Entity<Poll>()
                .HasIndex(p => p.CreatedBy)
                .HasDatabaseName("IX_Polls_CreatedBy");

            builder.Entity<PollOption>()
                .HasIndex(po => po.PollId)
                .HasDatabaseName("IX_PollOptions_PollId");
        }
    }
}
