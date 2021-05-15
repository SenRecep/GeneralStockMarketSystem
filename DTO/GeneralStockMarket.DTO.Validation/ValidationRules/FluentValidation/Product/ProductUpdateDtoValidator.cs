
using FluentValidation;

using GeneralStockMarket.DTO.Product;

namespace GeneralStockMarket.DTO.Validation.ValidationRules.FluentValidation.Product
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.UpdateUserId).NotEmpty().WithMessage("UserID is required");
        }
    }
}
