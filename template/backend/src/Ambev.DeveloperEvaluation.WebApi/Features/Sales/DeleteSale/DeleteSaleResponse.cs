using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// API Response model for delete sale operation
/// </summary>
[ExcludeFromCodeCoverage]
public class DeleteSaleResponse
{
    /// <summary>
    /// Indicates if the deletion was successful
    /// </summary>
    public bool Success { get; set; }
}