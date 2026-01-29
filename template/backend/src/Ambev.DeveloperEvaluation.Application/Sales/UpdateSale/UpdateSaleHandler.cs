using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting UpdateSaleCommand for customer {CustomerId}", command.CustomerId);
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        var sale = new Sale();
        sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            return _mapper.Map<UpdateSaleResult>(sale);

        if (sale.IsCancelled)
            return _mapper.Map<UpdateSaleResult>(sale);

        var itemsToUpdate = command.Items.Select(i => (i.ProductId, i.ProductName, i.Quantity, i.UnitPrice)).ToList();

        sale.ToUpdate(command.CustomerId, command.CustomerName, command.BranchId, command.BranchName, itemsToUpdate);

        await _saleRepository.UpdateAsync(sale, cancellationToken);
        _logger.LogInformation("Sending event [SaleModified] for Sale ID {Id}", command.Id);

        _logger.LogInformation("Finishing UpdateSaleCommand for customer {CustomerId}", command.CustomerId);
        return _mapper.Map<UpdateSaleResult>(sale);
    }
}