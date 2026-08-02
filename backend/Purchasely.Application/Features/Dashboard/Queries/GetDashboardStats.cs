using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Dashboard.Queries;

public record GetDashboardStatsQuery() : IRequest<Result<DashboardStatsResponse>>;

public class GetDashboardStatsQueryHandler(
    IRequisitionRepository requisitionRepo,
    IPurchaseOrderRepository purchaseOrderRepo,
    ICacheService cache
) : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsResponse>>
{
    private const string CacheKey = "dashboard:stats";

    public async Task<Result<DashboardStatsResponse>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var cached = await cache.GetAsync<DashboardStatsResponse>(CacheKey, cancellationToken);
        if (cached is not null)
            return Result<DashboardStatsResponse>.Success(cached);

        var totalRequisitions = await requisitionRepo.CountAsync(cancellationToken);
        var pendingApprovals = await requisitionRepo.CountByStatusAsync([RequisitionStatus.Submitted], cancellationToken);
        var openPOs = await purchaseOrderRepo.CountByStatusAsync([PurchaseOrderStatus.Draft, PurchaseOrderStatus.Issued, PurchaseOrderStatus.PartiallyReceived, PurchaseOrderStatus.Received], cancellationToken);
        var totalSpend = await purchaseOrderRepo.TotalSpendThisMonthAsync(cancellationToken);

        var stats = new DashboardStatsResponse(
            totalRequisitions,
            pendingApprovals,
            openPOs,
            totalSpend);

        await cache.SetAsync(CacheKey, stats, TimeSpan.FromMinutes(5), cancellationToken);

        return Result<DashboardStatsResponse>.Success(stats);
    }
}