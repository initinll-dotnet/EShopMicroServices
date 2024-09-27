using BuildingBlocks.CQRS;

using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;


public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create product entity
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // save to db

        // return result
        var result = new CreateProductResult
        {
            Id = Guid.NewGuid()
        };

        return result;
    }
}

public record CreateProductCommand : ICommand<CreateProductResult>
{
    public string Name { get; init; }
    public List<string> Category { get; init; }
    public string Description { get; init; }
    public string ImageFile { get; init; }
    public decimal Price { get; init; }
}

public record CreateProductResult
{
    public Guid Id { get; init; }
}
