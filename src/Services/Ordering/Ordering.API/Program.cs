using BuildingBlocks.Telemetry;

using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ########### Add services to the container ###########

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

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

// ########### Configure HTTP request pipeline ###########

app.Run();
