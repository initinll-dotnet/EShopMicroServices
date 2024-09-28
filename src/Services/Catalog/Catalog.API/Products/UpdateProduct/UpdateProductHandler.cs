namespace Catalog.API.Products.UpdateProduct;

internal class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IDocumentSession session;
    private readonly ILogger<UpdateProductCommandHandler> logger;

    public UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
    {
        this.session = session;
        this.logger = logger;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Command}", command);

        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException();
        }

        product.Name = command.Name;
        product.Description = command.Description;
        product.Category = command.Category;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        var result = new UpdateProductResult
        {
            IsSuccess = true
        };

        return result;
    }
}

public record UpdateProductCommand : ICommand<UpdateProductResult>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<string> Category { get; init; }
    public string Description { get; init; }
    public string ImageFile { get; init; }
    public decimal Price { get; init; }
}

public record UpdateProductResult
{
    public bool IsSuccess { get; init; }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

        RuleFor(command => command.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}


