namespace Catalog.API.Products.GetProductsByCategory;

internal class GetProductsByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    private readonly IDocumentSession session;
    private readonly ILogger<GetProductsByCategoryQueryHandler> logger;

    public GetProductsByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductsByCategoryQueryHandler> logger)
    {
        this.session = session;
        this.logger = logger;
    }

    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsByCategoryQueryHandler.Handle called with {@Query}", query);

        var products = await session
            .Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);

        if (products is null)
        {
            Results.NotFound();
        }

        var result = new GetProductsByCategoryResult
        {
            Products = products
        };

        return result;
    }
}

public record GetProductsByCategoryQuery : IQuery<GetProductsByCategoryResult>
{
    public string Category { get; init; }
}

public record GetProductsByCategoryResult
{
    public IEnumerable<Product> Products { get; init; }
}
