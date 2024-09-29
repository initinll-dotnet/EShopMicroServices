using Microsoft.Extensions.Caching.Distributed;

using System.Text.Json;

namespace Basket.API.Data;

public class CachedBasketRepository : IBasketRepository
{
    private readonly IBasketRepository basketRepository;
    private readonly IDistributedCache distributedCache;

    public CachedBasketRepository([FromKeyedServices("db")] IBasketRepository basketRepository, IDistributedCache distributedCache)
    {
        this.basketRepository = basketRepository;
        this.distributedCache = distributedCache;
    }

    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        // get from cache
        var cachedBasket = await distributedCache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            // if found, return from cache
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        // if not found in cache, get from db
        var basket = await basketRepository.GetBasket(userName, cancellationToken);
        // set in cache
        await distributedCache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        // return
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        // store in db
        await basketRepository.StoreBasket(basket, cancellationToken);

        // also set in cache
        await distributedCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        // delete from db
        await basketRepository.DeleteBasket(userName, cancellationToken);

        // deleted from cache too
        await distributedCache.RemoveAsync(userName, cancellationToken);

        return true;
    }
}
