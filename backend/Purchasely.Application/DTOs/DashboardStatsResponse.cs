namespace Purchasely.Application.DTOs;

public record DashboardStatsResponse(
    int TotalRequisitions,
    int PendingApprovals,
    int OpenPOs,
    decimal TotalSpend
);