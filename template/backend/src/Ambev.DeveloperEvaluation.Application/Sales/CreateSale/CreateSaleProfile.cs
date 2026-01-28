using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

[ExcludeFromCodeCoverage]
public class CreateSaleProfile : Profile
{
    public CreateSaleProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ConstructUsing(src => new Sale(src.CustomerId, src.CustomerName, src.BranchId, src.BranchName));
        
        CreateMap<CreateSaleItemDto, SaleItem>()
            .ConstructUsing(src => new SaleItem(src.ProductId, src.ProductName, src.Quantity, src.UnitPrice));
        
        CreateMap<Sale, CreateSaleResult>();
    }
}
