using BuildingBlocks.Telemetry;

using Discount.Grpc.Data;
using Discount.Grpc.Services;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database")!;

// ########### Add services to the container ###########

builder.Services.AddGrpc();
builder.Services
    .AddGrpcHealthChecks()
    .AddCheck(name: "Discount Grpc health", () => HealthCheckResult.Healthy());

builder.Services
    .AddHealthChecks();

builder.Services.AddDbContext<DiscountContext>(options =>
{
    options.UseSqlite(connectionString);
});

// Opentelemetry
builder.Services.AddOTelService(resourceName: "Discount.Grpc");
builder.Logging.AddOTelProvider();

// ########### Add services to the container ###########

var app = builder.Build();

// ########### Configure HTTP request pipeline ###########

if (builder.Environment.IsDevelopment())
{
    app.UseMigration();
}

app.MapGrpcService<DiscountService>();
app.MapGrpcHealthChecksService();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// ########### Configure HTTP request pipeline ###########

app.Run();
