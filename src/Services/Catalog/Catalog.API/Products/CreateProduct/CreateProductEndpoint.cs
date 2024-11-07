namespace Catalog.API.Products.CreateProduct;

public class CreateProductEndpoint : ICarterModule
{
    // carter
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, CancellationToken token, ISender sender) =>
        {
            // mapster
            var command = request.Adapt<CreateProductCommand>();
            // mediatr
            var result = await sender.Send(command, token);
            // mapster
            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}

public record CreateProductRequest
{
    public string Name { get; init; }
    public List<string> Category { get; init; }
    public string Description { get; init; }
    public string ImageFile { get; init; }
    public decimal Price { get; init; }
}

public record CreateProductResponse
{
    public Guid Id { get; init; }
}
