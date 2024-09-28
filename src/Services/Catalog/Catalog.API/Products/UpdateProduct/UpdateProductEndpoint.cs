namespace Catalog.API.Products.UpdateProduct;

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, CancellationToken token, ISender sender) =>
        {
            try
            {
                var command = request.Adapt<UpdateProductCommand>();

                var result = await sender.Send(command, token);

                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            }
            catch (ProductNotFoundException ex)
            {
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("UpdateProduct")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Product")
        .WithDescription("Update Product");
    }
}

public record UpdateProductRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<string> Category { get; init; }
    public string Description { get; init; }
    public string ImageFile { get; init; }
    public decimal Price { get; init; }
}

public record UpdateProductResponse
{
    public bool IsSuccess { get; init; }
}