using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.Features.Suppliers.Commands;

public record CreateSupplierProductCommand(
    Guid SupplierId,
    Guid ProductId,
    decimal UnitPrice
) : IRequest<Result<SupplierProductResponse>>;

public class CreateSupplierProductCommandHandler(
    ISupplierRepository supplierRepo,
    IProductRepository productRepo,
    ISupplierProductRepository supplierProductRepo
) : IRequestHandler<CreateSupplierProductCommand, Result<SupplierProductResponse>>
{
    public async Task<Result<SupplierProductResponse>> Handle(CreateSupplierProductCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepo.GetByIdAsync(request.SupplierId, cancellationToken);
        if (supplier is null)
            return Result<SupplierProductResponse>.Failure(404, "Supplier does not exist");

        var product = await productRepo.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result<SupplierProductResponse>.Failure(404, "Product does not exist");

        var exists = await supplierProductRepo.ExistsForSupplierAndProduct(request.SupplierId, request.ProductId, cancellationToken);
        if (exists)
            return Result<SupplierProductResponse>.Failure(404, "Product is already registerd for supplier");

        var supplierProduct = SupplierProduct.Create(request.SupplierId, request.ProductId, request.UnitPrice);
        
        await supplierProductRepo.AddAsync(supplierProduct, cancellationToken);
        bool saved = await supplierProductRepo.SaveChangesAsync(cancellationToken);

        return saved 
            ? Result<SupplierProductResponse>.Success(new SupplierProductResponse(supplier.Id, product.Id, supplierProduct.UnitPrice))
            : Result<SupplierProductResponse>.Failure(400, "Failed saving in database");
    }
}