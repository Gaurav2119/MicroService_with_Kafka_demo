using FluentValidation;
using InventoryService.Models;

namespace InventoryService.Validations
{
    public class InventoryValidations : AbstractValidator<Inventory>
    {
        public InventoryValidations()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty).WithMessage("Id is required");
            RuleFor(x => x.productName).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to zero");
        }
    }
}
