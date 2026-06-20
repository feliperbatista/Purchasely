using AutoMapper;
using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.Features.Products.Commands;

public record CreateProductCommand(
    string SKU,
    string Name,
    decimal UnitPrice,
    string? Description,
    Guid SupplierId
) : IRequest<Result<ProductResponse>>;

public class CreateProductCommandHandler(
    IProductRepository productRepo,
    ISupplierRepository supplierRepo,
    IMapper mapper
) : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepo.GetByIdAsync(request.SupplierId, cancellationToken);

        if (supplier is null)
            return Result<ProductResponse>.Failure(404, "Supplier does not exist");

        bool exists = await productRepo.ExistsBySupplierAndSKUAsync(request.SupplierId, request.SKU, cancellationToken);

        if (exists)
            return Result<ProductResponse>.Failure(409, "SKU already exists for this supplier");

        var product = Product.Create(request.SKU, request.Name, request.UnitPrice, request.Description, request.SupplierId);
        
        await productRepo.AddAsync(product, cancellationToken);
        await productRepo.SaveChangesAsync(cancellationToken);

        return Result<ProductResponse>.Success(mapper.Map<ProductResponse>(product));
    }
}