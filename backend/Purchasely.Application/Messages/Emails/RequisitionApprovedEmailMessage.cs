namespace Purchasely.Application.Messages.Emails;

public record RequisitionApprovedEmailMessage(
    Guid RequisitionId,
    int RequisitionNumber,
    string RequesterEmail,
    string RequesterName,
    string ApproverName
);