using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string ProductName { get; private set; }

    
    public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 identical items.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;

        CalculateDiscount();
        CalculateTotal();
    }

    private void CalculateDiscount()
    {
        if (Quantity >= 10)
        {
            Discount = 0.20m; // 20OFF
        }
        else if (Quantity >= 4)
        {
            Discount = 0.10m; // 10OFF
        }
        else
        {
            Discount = 0.00m;
        }
    }

    private void CalculateTotal()
    {
        var grossAmount = Quantity * UnitPrice;
        TotalAmount = grossAmount - (grossAmount * Discount);
    }
}