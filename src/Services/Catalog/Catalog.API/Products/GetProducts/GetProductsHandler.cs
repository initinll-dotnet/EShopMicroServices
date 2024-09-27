namespace Catalog.API.Products.GetProducts;

internal class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession session;
    private readonly ILogger<GetProductsQueryHandler> logger;

    public GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    {
        this.session = session;
        this.logger = logger;
    }

    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);

        var products = await session
            .Query<Product>()
            .ToListAsync(cancellationToken);

        var result = new GetProductsResult
        {
            Products = products
        };

        return result;
    }
}

public record GetProductsQuery : IQuery<GetProductsResult>
{
}

public record GetProductsResult
{
    public IEnumerable<Product> Products { get; init; }
}
