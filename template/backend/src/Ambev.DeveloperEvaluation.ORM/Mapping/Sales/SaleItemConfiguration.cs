using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.ORM.Mapping.Sales;

[ExcludeFromCodeCoverage]
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");
        builder.HasKey(u => u.Id);
        builder.Property(si => si.ProductId).IsRequired();
        builder.Property(si => si.ProductName).IsRequired().HasMaxLength(100);
        builder.Property(si => si.Quantity).IsRequired();
        builder.Property(si => si.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.Discount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.TotalSaleItemAmount).IsRequired().HasColumnType("decimal(18,2)");

        builder.HasIndex(si => si.ProductId);
    }
}