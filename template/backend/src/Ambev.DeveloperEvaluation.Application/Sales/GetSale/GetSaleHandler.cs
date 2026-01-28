using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleHandler : IRequestHandler<GetSaleQuery, GetSaleResult>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetSaleHandler> _logger;
        private readonly ISaleRepository _saleRepository;

        public GetSaleHandler(IMapper mapper, ILogger<GetSaleHandler> logger, ISaleRepository saleRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _saleRepository = saleRepository;
        }

        public async Task<GetSaleResult> Handle(GetSaleQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetSaleQuery for Sale ID {SaleId}", query.Id);
            var sale = await _saleRepository.GetByIdAsync(query.Id, cancellationToken);

            if (sale == null)
                _logger.LogInformation("Sale with ID {SaleId} was not found.", query.Id);

            _logger.LogInformation("Ending GetSaleQuery for Sale ID {SaleId}", query.Id);
            return _mapper.Map<GetSaleResult>(sale);
        }
    }
}
