
using FluentValidation;

using GeneralStockMarket.DTO.Trade;

namespace GeneralStockMarket.DTO.Validation.ValidationRules.FluentValidation.Trade
{
    public class TradeCreateDtoValidator : AbstractValidator<TradeCreateDto>
    {
        public TradeCreateDtoValidator()
        {
            When(x => x.TradeType == General.TradeType.Buy, () =>
            {
                RuleFor(x => x.Amount).ExclusiveBetween(0.0001, double.MaxValue).WithMessage("Aralık dışı");
            });

            When(x => x.TradeType == General.TradeType.Sell, () =>
            {
                RuleFor(x => x.Amount).ExclusiveBetween(0.0001, double.MaxValue).WithMessage("Aralık dışı");
                RuleFor(x => x.UnitPrice).ExclusiveBetween(0.0001, double.MaxValue).WithMessage("Aralık dışı");
            });
        }
    }
}
