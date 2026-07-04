namespace Purchasely.Application.Messages.Emails;

public record PurchaseOrderIssuedEmailMessage(
    Guid PurchaseOrderId,
    int PoNumber,
    string SupplierEmail,
    string SupplierName,
    decimal TotalAmount
);