using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Products.Commands;

public record DeleteProductCommand(
    Guid Id
) : IRequest<Result<Unit>>;

public class DeleteProductCommandHandler(
    IProductRepository repository
) : IRequestHandler<DeleteProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
            return Result<Unit>.Failure(404, "Product not found");

        repository.Delete(product);
        await repository.SaveChangesAsync(cancellationToken);
        
        return Result<Unit>.Success(Unit.Value);
    }
}