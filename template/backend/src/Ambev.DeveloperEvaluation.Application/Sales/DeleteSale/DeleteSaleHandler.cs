using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Handler for processing DeleteSaleCommand
/// </summary>
public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<DeleteSaleHandler> _logger;

    public DeleteSaleHandler(ISaleRepository saleRepository, ILogger<DeleteSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    public async Task<DeleteSaleResult> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting DeleteSaleCommand for Sale ID {Id}", command.Id);
        var sale = new Sale();
        sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} was not found");

        var success = await _saleRepository.DeleteAsync(command.Id, cancellationToken);
        _logger.LogInformation("Sending event [SaleModified] for Sale ID {Id}", command.Id);
        _logger.LogInformation("Finishing DeleteSaleCommand for Sale ID {Id}", command.Id);
        return new DeleteSaleResult { Success = success };
    }
}