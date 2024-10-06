using Refit;
using Shopping.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var apiGatewayAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services
    .AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(c => c.BaseAddress = apiGatewayAddress);

builder.Services
    .AddRefitClient<IBasketService>()
    .ConfigureHttpClient(c => c.BaseAddress = apiGatewayAddress);

builder.Services
    .AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(c => c.BaseAddress = apiGatewayAddress);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
