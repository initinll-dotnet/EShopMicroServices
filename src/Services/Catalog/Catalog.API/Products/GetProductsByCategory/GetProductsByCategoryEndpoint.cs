namespace Catalog.API.Products.GetProductsByCategory;

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, CancellationToken token, ISender sender) =>
        {
            // mapster
            var query = new GetProductsByCategoryQuery
            {
                Category = category
            };

            // mediatr
            var result = await sender.Send(query, token);

            // mapster
            var response = result.Adapt<GetProductsByCategoryResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductsByCategory")
        .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products By Category")
        .WithDescription("Get Products By Category");
    }
}

//public record GetProductsByCategoryRequest
//{
//}

public record GetProductsByCategoryResponse
{
    public IEnumerable<Product> Products { get; init; }
}
