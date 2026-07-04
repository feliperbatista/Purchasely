namespace Purchasely.Application.Messages.Emails;

public record RequisitionRejectedEmailMessage(
    Guid RequisitionId,
    int RequisitionNumber,
    string RequesterEmail,
    string RequesterName,
    string Reason
);