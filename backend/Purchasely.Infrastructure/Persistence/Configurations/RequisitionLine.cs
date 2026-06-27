using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchasely.Domain.Entities;

namespace Purchasely.Infrastructure.Persistence.Configurations;

public class RequisitionLineConfiguration : IEntityTypeConfiguration<RequisitionLine>
{
    public void Configure(EntityTypeBuilder<RequisitionLine> builder)
    {
        builder.ToTable("requisition_lines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EstimatedUnitPrice)
            .HasPrecision(18, 2);

        builder.HasOne(x => x.Requisition)
            .WithMany(r => r.Lines)
            .HasForeignKey(x => x.RequisitionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}