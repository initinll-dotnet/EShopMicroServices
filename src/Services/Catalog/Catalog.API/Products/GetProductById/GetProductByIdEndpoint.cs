namespace Catalog.API.Products.GetProductById;

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, CancellationToken token, ISender sender) =>
        {
            // mapster
            var query = new GetProductByIdQuery
            {
                Id = id
            };

            // mediatr
            var result = await sender.Send(query, token);

            // mapster
            var response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}

//public record GetProductByIdRequest
//{
//}

public record GetProductByIdResponse
{
    public Product Product { get; init; }
}
