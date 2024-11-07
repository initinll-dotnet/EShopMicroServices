namespace Basket.API.Basket.StoreBasket;

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, CancellationToken token, ISender sender) =>
        {
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command, token);
            var response = result.Adapt<StoreBasketResponse>();

            return Results.Created($"/basket/{response.UserName}", response);
        })
        .WithName("StoreBasket")
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Store Basket")
        .WithDescription("Store Basket");
    }
}

public record StoreBasketRequest
{
    public ShoppingCart Cart { get; init; }
}

public record StoreBasketResponse
{
    public string UserName { get; init; }
}
