using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Products.Queries;

public record GetProductsQuery : IRequest<Result<List<ProductResponse>>>;

public class GetProductsQueryHandler(
    IProductRepository repository
) : IRequestHandler<GetProductsQuery, Result<List<ProductResponse>>>
{
    public async Task<Result<List<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return Result<List<ProductResponse>>.Success([.. products.Select(p =>
            new ProductResponse(p.Id, p.SKU, p.Name, p.Description, p.Category))]);
    }
}