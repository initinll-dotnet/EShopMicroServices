namespace Basket.API.Basket.GetBasket;

internal class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    private readonly IBasketRepository basketRepository;

    public GetBasketQueryHandler([FromKeyedServices("cache")] IBasketRepository basketRepository)
    {
        this.basketRepository = basketRepository;
    }

    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasket(query.UserName, cancellationToken);

        return new GetBasketResult
        {
            Cart = basket
        };
    }
}

public record GetBasketQuery : IQuery<GetBasketResult>
{
    public string UserName { get; init; }
}

public record GetBasketResult
{
    public ShoppingCart Cart { get; init; }
}
