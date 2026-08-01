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
    ISupplierRepository repository
) : IRequestHandler<GetSuppliersQuery, Result<PaginatedResponse<SupplierListResponse>>>
{
    public async Task<Result<PaginatedResponse<SupplierListResponse>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await repository.GetAllAsync(request.Page, request.PageSize, request.Search, cancellationToken);
        var suppliersCount = await repository.CountAsync(cancellationToken);

        return Result<PaginatedResponse<SupplierListResponse>>.Success(new PaginatedResponse<SupplierListResponse>(
            [.. suppliers.Select(s => new SupplierListResponse(
                s.Id,
                s.Name,
                s.Email,
                s.Phone,
                s.IsActive
            ))],
            request.Page,
            request.PageSize,
            suppliersCount
        ));
    }
}