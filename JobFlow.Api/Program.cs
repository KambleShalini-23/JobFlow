using JobFlowApi.Data;
using JobFlowApi.Services;
using Microsoft.EntityFrameworkCore;
using JobFlowApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<CompanyService>();

builder.Services.AddScoped<JobService>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<JwtService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source= jobflow.db");
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "JobFlow API is running!");

app.MapControllers();

app.Run();