using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VotingSystem.API.Features.Auth.Services;
using VotingSystem.API.Features.User.Services;
using VotingSystem.API.Features.Voting.Services;
using VotingSystem.Domain.Entities;
using VotingSystem.Infrastructure.Data;
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
            services.AddIdentity<User, Role>(opts =>
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
            #endregion

            #region Register Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IVotingService, VotingService>();
            #endregion

            #region Register JWT
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero, // Prevent extra valid time after expiry
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                    };
                });
            #endregion

            #region CORS Policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            #endregion

            return services;
        }
    }
}
