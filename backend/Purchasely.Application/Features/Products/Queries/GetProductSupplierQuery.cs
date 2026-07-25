using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Products.Queries;

public record GetProductSupplierQuery(
    Guid ProductId
) : IRequest<Result<List<ProductSupplierResponse>>>;

public class GetProductSupplierQueryHandler(
    IProductRepository productRepository,
    ISupplierProductRepository supplierProductRepo
) : IRequestHandler<GetProductSupplierQuery, Result<List<ProductSupplierResponse>>>
{
    public async Task<Result<List<ProductSupplierResponse>>> Handle(GetProductSupplierQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            return Result<List<ProductSupplierResponse>>.Failure(404, "Product not found");

        var suppliers = await supplierProductRepo.GetProductSuppliersAsync(product.Id, cancellationToken);

        return Result<List<ProductSupplierResponse>>.Success([.. suppliers.Select(s => new ProductSupplierResponse(
            s.SupplierId,
            s.Supplier.Name,
            s.UnitPrice
        ))]);
    }
}