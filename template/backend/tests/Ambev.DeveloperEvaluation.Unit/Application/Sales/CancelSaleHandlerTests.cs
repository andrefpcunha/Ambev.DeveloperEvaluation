using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CancelSaleHandler>>();
        _handler = new CancelSaleHandler(_saleRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid sale ID When handling Then cancels sale and returns success")]
    public async Task Handle_ExistingSale_ShouldCancelAndReturnResult()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var sale = new Sale(Guid.NewGuid(), "Customer Test", Guid.NewGuid(), "Branch Test");

        sale.IsCancelled.Should().BeFalse();

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        var expectedResult = new CancelSaleResult { IsCancelled = true };
        _mapper.Map<CancelSaleResult>(sale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.IsCancelled.Should().BeTrue();

        await _saleRepository.Received(1).UpdateAsync(Arg.Is<Sale>(s => s.IsCancelled == true), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent sale ID When handling Then returns null")]
    public async Task Handle_NonExistentSale_ShouldReturnNull()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns((Sale?)null);
        _mapper.Map<CancelSaleResult>(null).Returns((CancelSaleResult?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeNull();
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given sale ID When handling Then should log starting and finishing information")]
    public async Task Handle_ExistingSale_ShouldLogInformation()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = new CancelSaleCommand(saleId);
        var sale = new Sale(Guid.NewGuid(), "Customer", Guid.NewGuid(), "Branch");

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _logger.ReceivedWithAnyArgs().LogInformation(default);
    }
}