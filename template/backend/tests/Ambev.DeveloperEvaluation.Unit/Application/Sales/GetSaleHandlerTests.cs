using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class GetSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;
    private readonly ISaleRepository _saleRepository;
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<GetSaleHandler>>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new GetSaleHandler(_mapper, _logger, _saleRepository);
    }

    [Fact(DisplayName = "Handle returns mapped result when sale exists")]
    public async Task Handle_SaleExists_ReturnsMappedResult()
    {
        // Given
        var saleId = Guid.NewGuid();
        var saleEntity = new Sale(Guid.NewGuid(), "Customer A", Guid.NewGuid(), "Branch X");
        saleEntity.AddItem(Guid.NewGuid(), "Product 1", 2, 10m);

        var expectedResult = new GetSaleResult
        {
            SaleNumber = "S-001",
            TotalSaleAmount = 20m,
            SaleDate = DateTime.UtcNow,
            CustomerId = saleEntity.CustomerId,
            CustomerName = saleEntity.CustomerName,
            BranchId = saleEntity.BranchId,
            BranchName = saleEntity.BranchName,
            Items = new List<GetSaleItemResult>()
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(saleEntity);
        _mapper.Map<GetSaleResult>(saleEntity).Returns(expectedResult);

        // When
        var result = await _handler.Handle(new GetSaleQuery(saleId), CancellationToken.None);

        // Then
        result.Should().BeSameAs(expectedResult);
        _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetSaleResult>(saleEntity);
    }

    [Fact(DisplayName = "Handle returns null when sale not found")]
    public async Task Handle_SaleNotFound_ReturnsNull()
    {
        // Given
        var id = Guid.NewGuid();
        _saleRepository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Sale?)null);
        _mapper.Map<GetSaleResult>(Arg.Any<Sale>()).Returns((GetSaleResult?)null);

        // When
        var result = await _handler.Handle(new GetSaleQuery(id), CancellationToken.None);

        // Then
        result.Should().BeNull();
        _saleRepository.Received(1).GetByIdAsync(id, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetSaleResult>(null);
    }
}