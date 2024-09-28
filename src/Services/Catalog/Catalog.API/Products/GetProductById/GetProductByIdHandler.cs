namespace Catalog.API.Products.GetProductById;

internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession session;

    public GetProductByIdQueryHandler(IDocumentSession session)
    {
        this.session = session;
    }

    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await session
            .LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(query.Id);
        }

        var result = new GetProductByIdResult
        {
            Product = product
        };

        return result;
    }
}

public record GetProductByIdQuery : IQuery<GetProductByIdResult>
{
    public Guid Id { get; init; }
}

public record GetProductByIdResult
{
    public Product Product { get; init; }
}
