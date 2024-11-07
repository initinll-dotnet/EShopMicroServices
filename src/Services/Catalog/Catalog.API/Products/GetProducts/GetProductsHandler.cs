namespace Catalog.API.Products.GetProducts;

internal class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession session;

    public GetProductsQueryHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session
            .Query<Product>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        var result = new GetProductsResult
        {
            Products = products
        };

        return result;
    }
}

public record GetProductsQuery : IQuery<GetProductsResult>
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}

public record GetProductsResult
{
    public IEnumerable<Product> Products { get; init; }
}
