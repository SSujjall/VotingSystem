using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using VotingSystem.API.DI;
using VotingSystem.API.Features.Hubs;
using VotingSystem.API.Mappers;
using VotingSystem.Common.Middlewares;
using VotingSystem.Common.ResponseModel;
using VotingSystem.Infrastructure.ExternalServices.EmailService.Config;
using VotingSystem.Infrastructure.ExternalServices.JwtService.Config;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Add Infrastructure FROM DI COntainer
builder.Services.AddInfrastructure(builder.Configuration);
#endregion

#region Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VotingSystem.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
#endregion

#region Authentication JWT Config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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

        // Custom message for when [Authorize] is used and user is not authorized or authenticated (no token is found in header)
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Skip the default logic.
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(ApiResponse<string>.Failed(
                    new Dictionary<string, string> { { "Authentication", "User not authenticated" } },
                    "Unauthenticated access"
                ));

                return context.Response.WriteAsync(result);
            }
        };
    });
#endregion

builder.Services.AddAuthorization();

#region Set Config to Models and Register service
// settings the values of jwt from configuration to the JWTSettings class
var jwtSection = builder.Configuration.GetSection("JWT");
builder.Services.Configure<JwtConfig>(jwtSection);

// settings the values of EmailConfiguration from configuration to the EmailConfig class
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
builder.Services.AddSingleton(emailConfig);
#endregion

#region Serilog Config
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
#endregion

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotingSystem API v1"));
}

//app.UseStaticFiles();


// Custom global error handling extension
app.UseErrorHandling();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Mapping votehub to endpoint
app.MapHub<VoteHub>("/votehub");

app.MapControllers();

app.Run();
