using MediatR;
using Purchasely.Application.Common;
using Purchasely.Application.DTOs;
using Purchasely.Application.Interfaces;
using Purchasely.Domain.Enums;

namespace Purchasely.Application.Features.Dashboard.Queries;

public record GetDashboardStatsQuery() : IRequest<Result<DashboardStatsResponse>>;

public class GetDashboardStatsQueryHandler(
    IRequisitionRepository requisitionRepo,
    IPurchaseOrderRepository purchaseOrderRepo
) : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsResponse>>
{
    public async Task<Result<DashboardStatsResponse>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var totalRequisitions = await requisitionRepo.CountAsync(cancellationToken);
        var pendingApprovals = await requisitionRepo.CountByStatusAsync([RequisitionStatus.Submitted], cancellationToken);
        var openPOs = await purchaseOrderRepo.CountByStatusAsync([PurchaseOrderStatus.Draft, PurchaseOrderStatus.Issued, PurchaseOrderStatus.PartiallyReceived, PurchaseOrderStatus.Received], cancellationToken);
        var totalSpend = await purchaseOrderRepo.TotalSpendThisMonthAsync(cancellationToken);

        return Result<DashboardStatsResponse>.Success(new DashboardStatsResponse(
            totalRequisitions,
            pendingApprovals,
            openPOs,
            totalSpend));
    }
}