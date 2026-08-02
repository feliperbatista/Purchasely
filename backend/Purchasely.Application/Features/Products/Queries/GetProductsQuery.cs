using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Products.Queries;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null
) : IRequest<Result<PaginatedResponse<ProductResponse>>>;

public class GetProductsQueryHandler(
    IProductRepository repository,
    ICacheService cache
) : IRequestHandler<GetProductsQuery, Result<PaginatedResponse<ProductResponse>>>
{
    private const string CacheKey = "products:all";

    public async Task<Result<PaginatedResponse<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var all = await cache.GetAsync<List<ProductResponse>>(CacheKey, cancellationToken);

        if (all is null)
        {
            var products = await repository.GetAllAsync(cancellationToken);
            all = [.. products
                .OrderBy(p => p.Name)
                .Select(p => new ProductResponse(
                    p.Id,
                    p.SKU,
                    p.Name,
                    p.Description,
                    p.Category,
                    p.CreatedAt))];

            await cache.SetAsync(CacheKey, all, TimeSpan.FromMinutes(30), cancellationToken);
        }

        var filtered = all.AsEnumerable();

        if (!string.IsNullOrEmpty(request.Search))
        {
            filtered = filtered.Where(p =>
                p.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                p.SKU.Contains(request.Search, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = filtered.Count();

        var items = filtered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var result = new PaginatedResponse<ProductResponse>(
            items,
            request.Page,
            request.PageSize,
            totalCount
        );
        
        return Result<PaginatedResponse<ProductResponse>>.Success(result);
    }
}