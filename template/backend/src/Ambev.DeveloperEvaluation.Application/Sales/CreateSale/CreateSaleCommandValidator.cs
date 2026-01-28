using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that enforces business rules for sale creation.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("The sale must contain at least one item.")
            .Must(items => items.Count > 0).WithMessage("The sale must have at least one product.");

        RuleForEach(x => x.Items).SetValidator(new CreateSaleItemValidator());

        RuleFor(x => x.Items)
            .Must(HaveValidQuantitiesPerProduct)
            .WithMessage("The total quantity for each identical product cannot exceed 20 units.");
    }

    private bool HaveValidQuantitiesPerProduct(List<CreateSaleItemDto> items)
    {
        if (items == null) return true;

        var totalsByProduct = items
            .GroupBy(i => i.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(i => i.Quantity) });

        return totalsByProduct.All(p => p.TotalQuantity <= 20);
    }
}
/// <summary>
/// Validator for individual sale items within the command.
/// </summary>
public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemDto>
{
    public CreateSaleItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
            .LessThanOrEqualTo(20).WithMessage("Quantity for a single item entry cannot exceed 20.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
    }
}