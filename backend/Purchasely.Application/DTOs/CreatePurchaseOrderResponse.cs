using Purchasely.Application.DTOs;

namespace Purchasely.Application.DTOs;

public record CreatePurchaseOrderResponse(
    Guid RequisitionId,
    List<PurchaseOrderResponse> PurchaseOrders
);