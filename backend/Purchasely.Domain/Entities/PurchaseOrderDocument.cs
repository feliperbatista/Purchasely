namespace Purchasely.Domain.Entities;

public class PurchaseOrderDocument
{
    public Guid Id { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required string BlobUrl { get; set; }
    public long FileSizeBytes { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid UploadedById { get; set; }

    private PurchaseOrderDocument() {}

    public static PurchaseOrderDocument Create(
        string fileName,
        string contentType,
        string blobUrl,
        long fileSizeBytes,
        Guid uploadedById)
    {
        return new PurchaseOrderDocument
        {
            FileName = fileName,
            ContentType = contentType,
            BlobUrl = blobUrl,
            FileSizeBytes = fileSizeBytes,
            UploadedById = uploadedById,
            UploadedAt = DateTime.UtcNow
        };
    }
}