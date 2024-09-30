var builder = WebApplication.CreateBuilder(args);

// ########### Add services to the container ###########

// ########### Add services to the container ###########

var app = builder.Build();

// ########### Configure HTTP request pipeline ###########

// ########### Configure HTTP request pipeline ###########

app.MapGet("/", () => "Hello World!");

app.Run();
