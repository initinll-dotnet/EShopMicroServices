namespace Basket.API.Basket.GetBasket;

internal class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        return new GetBasketResult
        {
            Cart = new ShoppingCart("swn")
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
