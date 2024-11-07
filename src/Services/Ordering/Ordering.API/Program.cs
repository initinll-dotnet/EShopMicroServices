using BuildingBlocks.Telemetry;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database")!;

// ########### Add services to the container ###########

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString: connectionString, name: "Ordering API SQLServer Health");

// Opentelemetry
builder.Services.AddOTelService(resourceName: "Ordering.API");
builder.Logging.AddOTelProvider();

// ########### Add services to the container ###########

var app = builder.Build();

// ########### Configure HTTP request pipeline ###########

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ########### Configure HTTP request pipeline ###########

app.Run();
