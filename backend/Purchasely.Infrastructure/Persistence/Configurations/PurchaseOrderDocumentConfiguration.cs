using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchasely.Domain.Entities;

namespace Purchasely.Infrastructure.Persistence.Configurations;

public class PurchaseOrderDocumentConfiguration : IEntityTypeConfiguration<PurchaseOrderDocument>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderDocument> builder)
    {
        builder.ToTable("purchase_order_documents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.ContentType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.BlobUrl).HasMaxLength(1000).IsRequired();

        builder.HasOne(x => x.PurchaseOrder)
            .WithMany(po => po.Documents)
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}