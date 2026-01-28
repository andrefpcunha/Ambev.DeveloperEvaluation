using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalSaleItemAmount { get; private set; }
    public string ProductName { get; private set; }

    
    public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;

        CalculateDiscount();
        CalculateTotalAmount();
    }

    private void CalculateDiscount()
    {
        if (Quantity >= 10 && Quantity <= 20)
        {
            Discount = 0.20m;
        }
        else if (Quantity > 4)
        {
            Discount = 0.10m;
        }
        else
        {
            Discount = 0.00m;
        }
    }

    private void CalculateTotalAmount()
    {
        var grossAmount = Quantity * UnitPrice;
        TotalSaleItemAmount = grossAmount - (grossAmount * Discount);
    }
}