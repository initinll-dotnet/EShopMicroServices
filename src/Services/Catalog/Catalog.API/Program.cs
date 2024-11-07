using BuildingBlocks.Telemetry;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("Database")!;

// ########### Add services to the container ###########

builder.Services.AddMediatR(config =>
{
    // mediatr pipeline
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(provider =>
{    
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string is not configured.");
    }
    provider.Connection(connectionString);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(connectionString: connectionString, name: "Catalog API PostgreSQL Health");

// Opentelemetry
builder.Services.AddOTelService(resourceName: "Catalog.API");
builder.Logging.AddOTelProvider();

// ########### Add services to the container ###########


var app = builder.Build();


// ########### Configure HTTP request pipeline ###########

app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// ########### Configure HTTP request pipeline ###########

app.Run();
