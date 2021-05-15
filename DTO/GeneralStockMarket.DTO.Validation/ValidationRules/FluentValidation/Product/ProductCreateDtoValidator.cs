
using FluentValidation;

using GeneralStockMarket.DTO.Product;

namespace GeneralStockMarket.DTO.Validation.ValidationRules.FluentValidation.Product
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
            RuleFor(x => x.CreatedUserId).NotEmpty().WithMessage("UserID is required");

        }
    }
}
