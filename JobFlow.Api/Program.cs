using JobFlowApi.Data;
using JobFlowApi.Services;
using Microsoft.EntityFrameworkCore;
using JobFlowApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<JobService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source= jobflow.db");
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapGet("/", () => "JobFlow API is running!");

app.MapControllers();

app.Run();