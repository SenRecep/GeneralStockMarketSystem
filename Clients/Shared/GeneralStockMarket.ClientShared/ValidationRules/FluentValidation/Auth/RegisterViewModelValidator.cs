
using FluentValidation;

using GeneralStockMarket.ClientShared.ViewModels;

namespace GeneralStockMarket.ClientShared.ValidationRules.FluentValidation.Auth
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş geçilemez")
                .EmailAddress().WithMessage("Geçersiz email formatı");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş geçilemez");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Parola boş geçilemez");

            RuleFor(x => x.PasswordConfirm)
              .Equal(x => x.Password).WithMessage("Parolalar eşleşmiyor");
        }
    }
}
