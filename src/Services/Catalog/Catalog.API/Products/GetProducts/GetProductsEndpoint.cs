namespace Catalog.API.Products.GetProducts;

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, CancellationToken token, ISender sender) =>
        {
            // mapster
            var query = request.Adapt<GetProductsQuery>();
            // mediatr
            var result = await sender.Send(query, token);
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

public record GetProductsRequest
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}

public record GetProductsResponse
{
    public IEnumerable<Product> Products { get; init; }
}
