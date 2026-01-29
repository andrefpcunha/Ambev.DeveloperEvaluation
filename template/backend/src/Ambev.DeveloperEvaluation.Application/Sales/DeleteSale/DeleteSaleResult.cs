using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Result data for delete sale command
/// </summary>
[ExcludeFromCodeCoverage]
public class DeleteSaleResult
{
    public bool Success { get; set; }
}