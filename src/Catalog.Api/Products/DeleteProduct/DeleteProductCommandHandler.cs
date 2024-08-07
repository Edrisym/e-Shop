using Catalog.Api.Products.CreateProduct;

namespace Catalog.Api.Products.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id)
    : ICommand<Result>;

public record DeleteProductResult(Result result);

public class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            return Result.Failure(ProductErrors.ProductNotFoundError);
        }
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}