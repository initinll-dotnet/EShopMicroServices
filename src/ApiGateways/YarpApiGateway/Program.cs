using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Disable HTTPS and map to the ports defined in Docker Compose
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Map to Docker port 6004 in docker compose
    options.ListenAnyIP(8081); // Map to Docker port 6064 in docker compose
});

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

app.UseRateLimiter();

app.MapReverseProxy();

app.Run();
