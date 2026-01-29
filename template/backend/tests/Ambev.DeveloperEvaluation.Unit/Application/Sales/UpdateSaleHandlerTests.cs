using Ambev.DeveloperEvaluation.Application.Sales.Dtos;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<UpdateSaleHandler>>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _logger);
    }

    [Fact(DisplayName = "Given valid command When sale exists and not cancelled Then updates and returns success")]
    public async Task Handle_ValidCommand_UpdatesAndReturnsResult()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = CreateUpdateSaleCommand(saleId);
        var existingSale = new Sale(command.CustomerId, "Old Name", command.BranchId, "Old Branch");

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(existingSale);

        var expectedResult = new UpdateSaleResult { Id = saleId };
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        _logger.ReceivedWithAnyArgs().LogInformation(default);
    }

    [Fact(DisplayName = "Given invalid command When validation fails Then throws ValidationException")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var command = new UpdateSaleCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given non-existent sale id When handling Then returns null result")]
    public async Task Handle_SaleNotFound_ReturnsMappedNull()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = CreateUpdateSaleCommand(saleId);

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns((Sale?)null);
        _mapper.Map<UpdateSaleResult>(null).Returns((UpdateSaleResult?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeNull();
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given cancelled sale When handling Then returns result without updating")]
    public async Task Handle_CancelledSale_ReturnsMappedSaleWithoutUpdate()
    {
        // Given
        var saleId = Guid.NewGuid();
        var command = CreateUpdateSaleCommand(saleId);

        var cancelledSale = new Sale(command.CustomerId, "Customer", command.BranchId, "Branch");
        cancelledSale.Cancel(); // IsCancelled = true

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(cancelledSale);

        var expectedResult = new UpdateSaleResult { Id = saleId, IsCancelled = true };
        _mapper.Map<UpdateSaleResult>(cancelledSale).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.IsCancelled.Should().BeTrue();

        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    private UpdateSaleCommand CreateUpdateSaleCommand(Guid id)
    {
        return new UpdateSaleCommand
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            CustomerName = "New Customer",
            BranchId = Guid.NewGuid(),
            BranchName = "New Branch",
            Items = new List<UpdateSaleItemDto>
            {
                new() { ProductId = Guid.NewGuid(), ProductName = "Product 1", Quantity = 5, UnitPrice = 10 }
            }
        };
    }
}