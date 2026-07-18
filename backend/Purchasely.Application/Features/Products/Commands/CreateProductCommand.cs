using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Entities;

namespace Purchasely.Application.Features.Products.Commands;

public record CreateProductCommand(
    string SKU,
    string Name,
    string? Description,
    string? Category
) : IRequest<Result<ProductResponse>>;

public class CreateProductCommandHandler(
    IProductRepository productRepo
) : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(request.SKU, request.Name, request.Description, request.Category);
        
        await productRepo.AddAsync(product, cancellationToken);
        var saved = await productRepo.SaveChangesAsync(cancellationToken);

        return saved 
            ? Result<ProductResponse>.Success(new ProductResponse(product.Id, product.SKU, product.Name, product.Description, product.Category, product.CreatedAt))
            : Result<ProductResponse>.Failure(400, "Failed saving in database");
    }
}