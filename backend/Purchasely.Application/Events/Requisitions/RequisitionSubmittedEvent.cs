using MediatR;

namespace Purchasely.Application.Events.Requisitions;

public record RequisitionSubmittedEvent(
    Guid RequisitionId,
    Guid RequesterId,
    DateTime SubmittedAt,
    string RequesterName,
    int RequisitionNumber,
    List<string> ApproverEmails
) : INotification;