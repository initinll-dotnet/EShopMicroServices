
namespace Catalog.API.Products.DeleteProduct;

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession session;
    private readonly ILogger<DeleteProductCommandHandler> logger;

    public DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    {
        this.session = session;
        this.logger = logger;
    }

    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);

        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);

        var result = new DeleteProductResult
        {
            IsSuccess = true
        };

        return result;
    }
}

public record DeleteProductCommand : ICommand<DeleteProductResult>
{
    public Guid Id { get; init; }
}

public record DeleteProductResult
{
    public bool IsSuccess { get; init; }
}

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
    }
}
