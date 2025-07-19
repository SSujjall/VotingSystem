using Microsoft.OpenApi.Models;
using VotingSystem.API.DI;
using VotingSystem.Common.Middlewares;
using VotingSystem.Infrastructure.ExternalServices.EmailService.Config;
using VotingSystem.Infrastructure.ExternalServices.JwtService.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

#region Add Infrastructure FROM DI COntainer
builder.Services.AddInfrastructure(builder.Configuration);
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotingSystem API v1"));
}

// Custom global error handling extension
app.UseErrorHandling();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
