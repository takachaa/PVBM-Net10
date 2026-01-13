using Scalar.AspNetCore;
using Serilog;
using Api.Middlewares;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Services
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddDomainServices()
    .AddCookieAuthentication();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS
var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(',') ?? [];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    // マイグレーションとシード
    await app.Services.ApplyMigrationsAndSeedAsync(app.Logger);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
