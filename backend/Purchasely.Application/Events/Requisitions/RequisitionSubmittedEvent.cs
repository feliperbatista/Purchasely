using MediatR;

namespace Purchasely.Application.Events.Requisitions;

public record RequisitionSubmittedEvent(
    Guid RequisitionId,
    Guid SubmittedById,
    DateTime SubmittedAt
) : INotification;