using Discount.Grpc.Data;
using Discount.Grpc.Services;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database")!;

// ########### Add services to the container ###########

builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(options =>
{
    options.UseSqlite(connectionString);
});

// ########### Add services to the container ###########

var app = builder.Build();

// ########### Configure HTTP request pipeline ###########

if (builder.Environment.IsDevelopment())
{
    app.UseMigration();
}
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// ########### Configure HTTP request pipeline ###########

app.Run();
