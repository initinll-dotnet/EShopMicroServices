namespace Catalog.API.Products.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            // mapster
            var query = new GetProductsQuery();
            // mediatr
            var result = await sender.Send(query);
            // mapster
            var response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}

//public record GetProductsRequest();

public record GetProductsResponse
{
    public IEnumerable<Product> Products { get; init; }
}
