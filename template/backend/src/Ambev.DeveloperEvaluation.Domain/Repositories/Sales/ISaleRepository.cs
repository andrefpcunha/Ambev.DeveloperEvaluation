using Ambev.DeveloperEvaluation.Domain.Entities.Sale;

namespace Ambev.DeveloperEvaluation.Domain.Repositories.Sales
{
    /// <summary>
    /// Repository interface for Sale entity operations
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Persists a new sale in the database
        /// </summary>
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a sale by its internal identifier (GUID)
        /// </summary>
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a sale by its business identifier (SaleNumber)
        /// </summary>
        Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing sale
        /// </summary>
        Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a sale from the database
        /// </summary>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
