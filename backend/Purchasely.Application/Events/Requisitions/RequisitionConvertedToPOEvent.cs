using MediatR;

namespace Purchasely.Application.Events.Requisitions;

public record RequisitionConvertedToPOEvent(
    Guid RequisitionId,
    Guid ConvertedBy,
    DateTime ConvertedAt,
    List<Guid> POIds
) : INotification;

