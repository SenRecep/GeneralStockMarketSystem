using System.Collections.Generic;
using System.Threading.Tasks;


using GeneralStockMarket.AuthAPI.Models;
using GeneralStockMarket.CoreLib.Response;

using IdentityModel;

using IdentityServer4.Validation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GeneralStockMarket.AuthAPI.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            static Dictionary<string, object> errors()
            {
                return new Dictionary<string, object>
                {
                    { "Data",null },
                    { "StatusCode", StatusCodes.Status400BadRequest },
                    { "IsSuccessful", false },
                    { "ErrorData", Error.SendError(
                            path:"api/user/signin",
                            isShow:true,
                            errors:"Kullanıcı adı veya parolanız hatalı"
                        ) },
                };
            }

            ApplicationUser existUser = await userManager.FindByNameAsync(context.UserName);
            if (existUser is null)
            {
                context.Result.CustomResponse = errors();
                return;
            }
            bool passwordCheck = await userManager.CheckPasswordAsync(existUser, context.Password);
            if (passwordCheck is false)
            {
                context.Result.CustomResponse = errors();
                return;
            }

            context.Result = new GrantValidationResult(existUser.Id, OidcConstants.AuthenticationMethods.Password);
        }
    }
}
