using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchasely.Domain.Entities;

namespace Purchasely.Infrastructure.Persistence.Configurations;

public class RequisitionConfiguration : IEntityTypeConfiguration<Requisition>
{
    public void Configure(EntityTypeBuilder<Requisition> builder)
    {
        builder.ToTable("requisitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number)
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => x.Number)
            .IsUnique();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Priority)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.HasOne(x => x.Requester)
            .WithMany(r => r.Requisitions)
            .HasForeignKey(x => x.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}