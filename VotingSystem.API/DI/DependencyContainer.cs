using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingSystem.API.Features.Auth.Services;
using VotingSystem.API.Features.Polls.Services;
using VotingSystem.API.Features.UserProfile.Services;
using VotingSystem.API.Features.Voting.Services;
using VotingSystem.API.Features.VotingHistory.Services;
using VotingSystem.API.Mappers;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Data;
using VotingSystem.Infrastructure.ExternalServices.EmailService;
using VotingSystem.Infrastructure.ExternalServices.JwtService;
using VotingSystem.Infrastructure.Health;
using VotingSystem.Infrastructure.Repositories.Implementations;
using VotingSystem.Infrastructure.Repositories.Interfaces;

namespace VotingSystem.API.DI
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Db COnfig
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("VoteSysDb"));
            });
            #endregion

            #region Identity User and Role Config
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.SignIn.RequireConfirmedAccount = false;
                opts.Password.RequireDigit = false;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
            #endregion

            #region Register Repositories
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IPollRepository, PollRepository>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            #endregion

            #region Register Services
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IPollsService, PollsService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IVotingHistoryService, VotingHistoryService>();
            #endregion

            #region Register External Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            #endregion

            #region Autmapper Config
            services.AddAutoMapper(typeof(PollMappingProfile));
            #endregion

            #region Register Health Check configs
            services.AddHealthChecks()
                .AddCheck<DbHealthCheck>("Database");
            #endregion

            return services;
        }
    }
}
