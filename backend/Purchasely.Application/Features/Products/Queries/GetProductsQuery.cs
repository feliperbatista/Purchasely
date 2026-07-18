using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Products.Queries;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 20
) : IRequest<Result<PaginatedResponse<ProductResponse>>>;

public class GetProductsQueryHandler(
    IProductRepository repository
) : IRequestHandler<GetProductsQuery, Result<PaginatedResponse<ProductResponse>>>
{
    public async Task<Result<PaginatedResponse<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        var productsCount = await repository.CountAsync(cancellationToken);

        return Result<PaginatedResponse<ProductResponse>>.Success(new PaginatedResponse<ProductResponse>(
            [.. products.Select(p =>
            new ProductResponse(p.Id, p.SKU, p.Name, p.Description, p.Category, p.CreatedAt))],
            request.Page,
            request.PageSize,
            productsCount
        ));
    }
}