namespace Purchasely.Application.Messages.Emails;

public record RequisitionSubmittedEmailMessage(
    Guid RequisitionId,
    int RequisitionNumber,
    string RequesterName,
    List<string> ApproverEmails,
    DateTime SubmittedAt
);