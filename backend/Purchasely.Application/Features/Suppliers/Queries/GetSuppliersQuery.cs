using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;

namespace Purchasely.Application.Features.Suppliers.Queries;

public record GetSuppliersQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null
) : IRequest<Result<PaginatedResponse<SupplierListResponse>>>;

public class GetSuppliersQueryHandler(
    ISupplierRepository repository,
    ICacheService cache
) : IRequestHandler<GetSuppliersQuery, Result<PaginatedResponse<SupplierListResponse>>>
{
    private const string CacheKey = "suppliers:all";

    public async Task<Result<PaginatedResponse<SupplierListResponse>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var all = await cache.GetAsync<List<SupplierListResponse>>(CacheKey, cancellationToken);

        if (all is null)
        {
            var suppliers = await repository.GetAllAsync(cancellationToken);
            all = [.. suppliers
                .OrderBy(s => s.Name)
                .Select(s => new SupplierListResponse(
                    s.Id,
                    s.Name,
                    s.Email,
                    s.Phone,
                    s.IsActive
                ))];

            await cache.SetAsync(CacheKey, all, TimeSpan.FromMinutes(30), cancellationToken);
        }

        var filtered = all.AsEnumerable();

        if (!string.IsNullOrEmpty(request.Search))
        {
            filtered = filtered.Where(p =>
                p.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = filtered.Count();

        var items = filtered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return Result<PaginatedResponse<SupplierListResponse>>.Success(new PaginatedResponse<SupplierListResponse>(
            items,
            request.Page,
            request.PageSize,
            totalCount
        ));
    }
}