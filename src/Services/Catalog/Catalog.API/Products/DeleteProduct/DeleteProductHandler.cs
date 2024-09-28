
namespace Catalog.API.Products.DeleteProduct;

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession session;

    public DeleteProductCommandHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
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
