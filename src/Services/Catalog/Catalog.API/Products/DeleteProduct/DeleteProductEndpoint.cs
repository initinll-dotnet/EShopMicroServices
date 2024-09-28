namespace Catalog.API.Products.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, CancellationToken token, ISender sender) =>
        {
            var command = new DeleteProductCommand
            {
                Id = id
            };

            var result = await sender.Send(command, token);

            var response = result.Adapt<DeleteProductResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product");
    }
}

//public record DeleteProductRequest
//{
//    public Guid Id { get; init; }
//}

public record DeleteProductResponse
{
    public bool IsSuccess { get; init; }
}