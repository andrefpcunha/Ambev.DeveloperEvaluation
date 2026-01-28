using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Sales;

[ExcludeFromCodeCoverage]
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(20);
        builder.Property(s => s.CustomerName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.BranchName).IsRequired().HasMaxLength(100);

        builder.HasIndex(s => s.SaleNumber)
               .IsUnique();

        builder.HasMany(s => s.Items)
               .WithOne()
               .HasForeignKey("SaleId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}