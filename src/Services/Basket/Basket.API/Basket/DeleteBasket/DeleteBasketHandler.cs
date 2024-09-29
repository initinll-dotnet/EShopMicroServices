namespace Basket.API.Basket.DeleteBasket;

internal class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    private readonly IBasketRepository basketRepository;

    public DeleteBasketCommandHandler([FromKeyedServices("cache")] IBasketRepository basketRepository)
    {
        this.basketRepository = basketRepository;
    }

    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await basketRepository.DeleteBasket(command.UserName, cancellationToken);

        return new DeleteBasketResult
        {
            IsSuccess = result
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
