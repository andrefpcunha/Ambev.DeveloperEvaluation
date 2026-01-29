using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;

    public CancelSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting CancelSaleCommand for Sale ID {Id}", command.Id);
        var sale = new Sale();
        sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            return _mapper.Map<CancelSaleResult>(sale);
        
        sale.Cancel();
        sale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        var result = _mapper.Map<CancelSaleResult>(sale);
        _logger.LogInformation("Finishing CancelSaleCommand for Sale ID {Id}", command.Id);
        return result;
    }
}
