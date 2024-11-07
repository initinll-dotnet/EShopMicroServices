
using Basket.API.Basket.StoreBasket;

namespace Basket.API.Basket.DeleteBasket;

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, CancellationToken token, ISender sender) =>
        {
            var command = new DeleteBasketCommand
            {
                UserName = userName,
            };

            var result = await sender.Send(command, token);
            var response = result.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteBasket")
        .Produces<DeleteBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Delete Basket")
        .WithDescription("Delete Basket");
    }
}

//public record DeleteBasketRequest
//{
//    public string UserName { get; init; }
//}

public record DeleteBasketResponse
{
    public bool IsSuccess { get; init; }
}
