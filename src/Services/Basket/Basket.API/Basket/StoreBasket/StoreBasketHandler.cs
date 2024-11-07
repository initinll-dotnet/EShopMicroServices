using Discount.Grpc;

using static Discount.Grpc.DiscountProtoService;

namespace Basket.API.Basket.StoreBasket;

internal class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    private readonly IBasketRepository basketRepository;
    // discount grpc service
    private readonly DiscountProtoServiceClient discountProtoServiceClient;

    public StoreBasketCommandHandler(
        [FromKeyedServices("cache")] IBasketRepository basketRepository,
        DiscountProtoServiceClient discountProtoServiceClient)
    {
        this.basketRepository = basketRepository;
        // discount grpc service
        this.discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscount(command.Cart, cancellationToken);

        // upserting product with discounted amount
        await basketRepository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult
        {
            UserName = command.Cart.UserName
        };
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        var discountRequest = new GetDiscountRequest();

        // Communicate with Discount.Grpc and calculate lastest prices of products into sc
        foreach (var item in cart.Items)
        {
            discountRequest.ProductName = item.ProductName;

            // calling discount grpc service to get discount for product
            var coupon = await discountProtoServiceClient
                .GetDiscountAsync(discountRequest, cancellationToken: cancellationToken);

            // applying discount
            item.Price -= coupon.Amount;
        }
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