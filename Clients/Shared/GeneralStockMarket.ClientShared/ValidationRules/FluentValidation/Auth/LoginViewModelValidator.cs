
using FluentValidation;

using GeneralStockMarket.ClientShared.ViewModels;

namespace GeneralStockMarket.ClientShared.ValidationRules.FluentValidation.Auth
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş geçilemez");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola boş geçilemez");
        }
    }
}
