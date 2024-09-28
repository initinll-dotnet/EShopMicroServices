using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

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

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// ########### Add services to the container ###########


var app = builder.Build();


// ########### Configure HTTP request pipeline ###########

app.MapCarter();

app.UseExceptionHandler(options => { });


// ########### Configure HTTP request pipeline ###########

app.Run();
