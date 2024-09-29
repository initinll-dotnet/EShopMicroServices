namespace Basket.API.Basket.StoreBasket;

internal class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    private readonly IBasketRepository basketRepository;

    public StoreBasketCommandHandler([FromKeyedServices("cache")] IBasketRepository basketRepository)
    {
        this.basketRepository = basketRepository;
    }

    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await basketRepository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult
        {
            UserName = command.Cart.UserName
        };
    }
}

public record StoreBasketCommand : ICommand<StoreBasketResult>
{
    public ShoppingCart Cart { get; init; }
}

public record StoreBasketResult
{
    public string UserName { get; init; }
}

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}