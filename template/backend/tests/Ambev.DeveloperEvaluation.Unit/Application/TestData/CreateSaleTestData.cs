using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class CreateSaleTestData
{
    public static CreateSaleCommand GenerateCreateSaleCommand(int qtyValue, int qtyItems)
    {
        var itemFaker = new Faker<CreateSaleItemDto>()
            .RuleFor(x => x.ProductId, f => f.Random.Guid())
            .RuleFor(x => x.ProductName, f => f.Commerce.ProductName())
            .RuleFor(x => x.Quantity, f => qtyValue)
            .RuleFor(x => x.UnitPrice, f => decimal.Parse(f.Commerce.Price(10, 100)));

        return new Faker<CreateSaleCommand>()
            .RuleFor(c => c.CustomerId, f => f.Random.Guid())
            .RuleFor(c => c.CustomerName, f => f.Person.FullName)
            .RuleFor(c => c.BranchId, f => f.Random.Guid())
            .RuleFor(c => c.BranchName, f => f.Company.CompanyName())
            .RuleFor(c => c.Items, f => itemFaker.Generate(qtyItems));
    }
}
