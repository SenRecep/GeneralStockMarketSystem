
using System;

using FluentValidation;

using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.DTO.Request.Enums;

namespace GeneralStockMarket.DTO.Validation.ValidationRules.FluentValidation.Request
{
    public class GenereLRequestValidator : AbstractValidator<GenaralCreateDto>
    {
        public GenereLRequestValidator()
        {
            When(x => x.RequestType == RequestType.Deposit, () => {
                RuleFor(x => x.Amount).GreaterThan(0.0001).WithMessage("Aralık dışı");
                RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama giriniz");
            });

            When(x => x.RequestType == RequestType.Product, () => {
                RuleFor(x => x.ProductId).NotEqual(new Guid()).WithMessage("Ürün seçiniz");
                RuleFor(x => x.Amount).GreaterThan(0.0001).WithMessage("Aralık dışı");
                RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama giriniz");
            });

            When(x => x.RequestType == RequestType.NewType, () => {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Ürün adını giriniz");
                RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama giriniz");
            });
        }
    }
}
