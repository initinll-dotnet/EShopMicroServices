namespace Catalog.API.Products.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IDocumentSession session;

    public CreateProductCommandHandler(IDocumentSession session)
    {
        this.session = session;
    }

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
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // return result
        var result = new CreateProductResult
        {
            Id = product.Id
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
