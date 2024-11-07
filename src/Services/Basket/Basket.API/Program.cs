using BuildingBlocks.Telemetry;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var postgres_connectionString = builder.Configuration.GetConnectionString("Database")!;
var redis_connectionString = builder.Configuration.GetConnectionString("Redis")!;
var discountGrpcUrl = builder.Configuration["GrpcSettings:DiscountUrl"]!;

// ########### Add services to the container ###########

//Application Services
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    // mediatr pipeline
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

//Data Services
builder.Services.AddMarten(provider =>
{
    if (string.IsNullOrEmpty(postgres_connectionString))
    {
        throw new InvalidOperationException("Connection string is not configured.");
    }
    provider.Connection(postgres_connectionString);
    provider.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddKeyedScoped<IBasketRepository, BasketRepository>("db");
builder.Services.AddKeyedScoped<IBasketRepository, CachedBasketRepository>("cache");

builder.Services.AddStackExchangeRedisCache(options =>
{
    if (string.IsNullOrEmpty(redis_connectionString))
    {
        throw new InvalidOperationException("Connection string is not configured.");
    }
    options.Configuration = redis_connectionString;
});

//Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options => 
{
    options.Address = new Uri(discountGrpcUrl);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

//Async Communication Services (Publisher)
builder.Services.AddMessageBroker(builder.Configuration);

//Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(connectionString: postgres_connectionString, name: "Basket API PostgreSQL Health")
    .AddRedis(redisConnectionString: redis_connectionString, name: "Basket API Redis Health");

// Opentelemetry
builder.Services.AddOTelService(resourceName: "Basket.API");
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
