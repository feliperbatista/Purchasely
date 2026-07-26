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
    ISupplierProductRepository supplierProductRepo,
    ISupplierRepository supplierRepo
) : IRequestHandler<DeleteSupplierProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteSupplierProductCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepo.GetByIdAsync(request.SupplierId, cancellationToken);
        if (supplier is null)
            return Result<Unit>.Failure(404, "Supplier does not exist");

        var product = await supplierProductRepo.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result<Unit>.Failure(404, "Product does not exist");

        supplierProductRepo.Delete(product);
        await supplierProductRepo.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}