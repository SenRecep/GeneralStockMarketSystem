using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

using GeneralStockMarket.DTO.Trade;

namespace GeneralStockMarket.DTO.Validation.ValidationRules.FluentValidation.Trade
{
   public  class TradeCreateDtoValidator:AbstractValidator<TradeCreateDto>
    {
        public TradeCreateDtoValidator()
        {
            When(x=>x.TradeType==General.TradeType.Buy,()=> { 
                 RuleFor(x=>x.Amount).ExclusiveBetween(0.0001,double.MaxValue).WithMessage("Aralık dışı");
            });

            When(x => x.TradeType == General.TradeType.Sell, () => {
                RuleFor(x => x.Amount).ExclusiveBetween(0.0001, double.MaxValue).WithMessage("Aralık dışı");
                RuleFor(x => x.Price).ExclusiveBetween(0.0001, double.MaxValue).WithMessage("Aralık dışı");
            });
        }
    }
}
