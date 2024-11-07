namespace Catalog.API.Products.GetProductsByCategory;

internal class GetProductsByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    private readonly IDocumentSession session;

    public GetProductsByCategoryQueryHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
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
