namespace Catalog.API.Products.GetProductById;

internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession session;
    private readonly ILogger<GetProductByIdQueryHandler> logger;

    public GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    {
        this.session = session;
        this.logger = logger;
    }

    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler.Handle called with {@Query}", query);

        var product = await session
            .LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException();
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
