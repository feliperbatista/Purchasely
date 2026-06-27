using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.Features.Suppliers.Commands;

public record DeleteSupplierProductCommand(
    Guid SupplierId,
    Guid ProductId
) : IRequest<Result<Unit>>;

public class DeleteSupplierProductCommandHandler(
    ISupplierProductRepository repository
) : IRequestHandler<DeleteSupplierProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteSupplierProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdsAsync(request.SupplierId, request.ProductId, cancellationToken);
        if (product is null)
            return Result<Unit>.Failure(404, "Product does not exist");

        repository.Delete(product);
        bool saved = await repository.SaveChangesAsync(cancellationToken);

        return saved 
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(400, "Failed saving in database");
    }
}