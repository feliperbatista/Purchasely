using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductResponse>>;

public class GetProductByIdQueryHandler(
    IProductRepository repository
) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return Result<ProductResponse>.Failure(404, "Product not found");

        return Result<ProductResponse>.Success(new ProductResponse(product.Id, product.SKU, product.Name, product.Description, product.Category));
    }
}