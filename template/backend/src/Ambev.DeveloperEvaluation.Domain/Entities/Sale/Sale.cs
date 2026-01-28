using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Sale;

public class Sale : BaseEntity
{
    /// <summary>
    /// Business unique identifier for the sale
    /// </summary>
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal TotalSaleAmount { get; private set; }
    public bool IsCancelled { get; private set; }
    public List<SaleItem> Items { get; private set; }
    public string CustomerName { get; private set; }
    public string BranchName { get; private set; }

    public Sale()
    {
        Items = new List<SaleItem>();
    }

    public Sale(Guid customerId, string customerName, Guid branchId, string branchName)
    {
        GenerateSaleNumber();
        SaleDate = DateTime.UtcNow;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        Items = new List<SaleItem>();
        IsCancelled = false;
    }

    #region: Private Methods
    private void GenerateSaleNumber()
    {
        var datePart = DateTime.UtcNow.ToString("yyyyMM");
        var randomPart = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
        this.SaleNumber = $"SN-{datePart}-{randomPart}";
    }

    private void CalculateTotal()
    {
        TotalSaleAmount = Items.Sum(i => i.TotalSaleItemAmount);
    }
    #endregion

    #region Public Methods
    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        var item = new SaleItem(productId, productName, quantity, unitPrice);
        Items.Add(item);
        CalculateTotal();
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    #endregion
}
