
using FluentValidation;

using GeneralStockMarket.AuthAPI.Models;

namespace GeneralStockMarket.AuthAPI.Validations.FluentValidation
{
    public class SignUpViewModelValidation : AbstractValidator<SignUpViewModel>
    {
        public SignUpViewModelValidation()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email not valid");
            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required");
        }
    }
}
