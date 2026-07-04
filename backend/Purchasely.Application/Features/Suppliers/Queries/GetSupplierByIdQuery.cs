using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Suppliers.Queries;

public record GetSupplierByIdQuery(Guid Id) : IRequest<Result<SupplierDetailsResponse>>;

public class GetSupplierByIdQueryHandler(
    ISupplierRepository repository
) : IRequestHandler<GetSupplierByIdQuery, Result<SupplierDetailsResponse>>
{
    public async Task<Result<SupplierDetailsResponse>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (supplier is null)
            return Result<SupplierDetailsResponse>.Failure(404, "Supplier not found");

        return Result<SupplierDetailsResponse>.Success(new SupplierDetailsResponse
        (
            supplier.Id,
            supplier.Name,
            supplier.Email,
            supplier.Phone,
            supplier.TaxNumber,
            supplier.Address,
            supplier.IsActive,
            supplier.CreatedAt,
            supplier.Products.Select(sp => new SupplierProducts(
                sp.Product.SKU,
                sp.Product.Name,
                sp.UnitPrice,
                sp.Product.Description,
                sp.Product.Category
            ))));
    }
}