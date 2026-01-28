using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class CreateSaleHandlerTests
{
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly ISaleRepository _saleRepository;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new CreateSaleHandler(_mapper, _logger, _saleRepository);
    }

    [Fact(DisplayName = "Handle must have Expected Discount: 0%")]
    public async Task Handle_ValidCommand_Discount0()
    {
        // Given
        var command = CreateSaleTestData.GenerateCreateSaleCommand(qtyValue: 4, qtyItems: 1);
        var saleEntity = new Sale(command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);
        foreach (var item in command.Items)
        {
            saleEntity.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(saleEntity);

        // When
        await _handler.Handle(command, CancellationToken.None);

        //Then
        saleEntity.Items.All(i => i.Discount == 0m).Should().BeTrue();
    }

    [Fact(DisplayName = "Handle must have Expected Discount: 10%")]
    public async Task Handle_ValidCommand_Discount10()
    {
        // Given
        var command = CreateSaleTestData.GenerateCreateSaleCommand(qtyValue: 5, qtyItems: 1);
        var saleEntity = new Sale(command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);
        foreach (var item in command.Items)
        {
            saleEntity.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(saleEntity);

        // When
        await _handler.Handle(command, CancellationToken.None);

        //Then
        saleEntity.Items.All(i => i.Discount == 0.10m).Should().BeTrue();
    }

    [Fact(DisplayName = "Handle must have Expected Discount: 20%")]
    public async Task Handle_ValidCommand_Discount20()
    {
        // Given
        var command = CreateSaleTestData.GenerateCreateSaleCommand(qtyValue: 10, qtyItems: 1);
        var saleEntity = new Sale(command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);
        foreach (var item in command.Items)
        {
            saleEntity.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(saleEntity);

        // When
        await _handler.Handle(command, CancellationToken.None);

        //Then
        saleEntity.Items.All(i => i.Discount == 0.20m).Should().BeTrue();
    }

    [Fact(DisplayName = "Handle should throw ValidationException when qty of item > 20")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleTestData.GenerateCreateSaleCommand(qtyValue: 21, qtyItems: 1);

        // Then
        await Assert.ThrowsAsync<ValidationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}