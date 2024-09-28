
namespace Basket.API.Basket.DeleteBasket;

internal class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        return new DeleteBasketResult
        {
            IsSuccess = true
        };
    }
}

public record DeleteBasketCommand : ICommand<DeleteBasketResult>
{
    public string UserName { get; init; }
}

public record DeleteBasketResult
{
    public bool IsSuccess { get; init; }
}

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
    }
}
