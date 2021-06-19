using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Filters;
using GeneralStockMarket.AuthAPI.Dtos;
using GeneralStockMarket.AuthAPI.Models;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.CoreLib.StringInfo;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using static IdentityServer4.IdentityServerConstants;

namespace GeneralStockMarket.AuthAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)]
    public class UserController : CustomControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return GetResult(result, "SignUp");


            IdentityResult roleResult = await userManager.AddToRoleAsync(user, RoleInfo.Customer);

            if (!roleResult.Succeeded)
                return GetResult(roleResult, "SignUp_AddRole");

            return CreateResponseInstance(Response<NoContent>.Success(
                 statusCode: StatusCodes.Status201Created
                  ));
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            Claim userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if (userClaim.IsNull()) return CreateResponseInstance(Response<NoContent>.Fail(
                 statusCode: StatusCodes.Status400BadRequest,
                 isShow: true,
                 path: "api/User/GetUser",
                 errors: "Kullanici girisi dogrulanamadi"
                 ));

            ApplicationUser user = await userManager.FindByIdAsync(userClaim.Value);

            if (user.IsNull()) return CreateResponseInstance(Response<NoContent>.Fail(
                 statusCode: StatusCodes.Status400BadRequest,
                 isShow: true,
                 path: "api/User/GetUser",
                 errors: "Gecerli bir kullanici bulunamadi"
                 ));

            var dto = mapper.Map<ApplicationUserDto>(user);

            return CreateResponseInstance(Response<ApplicationUserDto>.Success(
                 data: dto,
                 statusCode: StatusCodes.Status200OK
                  ));
        }

        private IActionResult GetResult(IdentityResult result, string action = "UpdateProfile")
        {
            return CreateResponseInstance(Response<NoContent>.Fail(
                      statusCode: StatusCodes.Status400BadRequest,
                      isShow: true,
                      path: $"api/User/{action}",
                      errors: result.Errors.Select(x => x.Description).ToArray()
                      ));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(ApplicationUserDto model)
        {
            Claim userClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            ApplicationUser user = await userManager.FindByIdAsync(userClaim.Value);
            bool isVerified = await userManager.IsInRoleAsync(user, RoleInfo.IsVerified);


            var emailResult = await userManager.SetEmailAsync(user, model.Email);
            if (!emailResult.Succeeded)
                return GetResult(emailResult);

            var phoneNumberResult = await userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            if (!phoneNumberResult.Succeeded)
                return GetResult(emailResult);

            if (!isVerified)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.IdentityNumber = model.IdentityNumber;
            }

            user.Address = model.Address;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return GetResult(emailResult);


            if (!isVerified)
            {
                IdentityResult roleResult = await userManager.AddToRoleAsync(user, RoleInfo.IsVerified);

                if (!roleResult.Succeeded)
                    return GetResult(roleResult, "UpdateProfile_AddRole");
            }

            return CreateResponseInstance(Response<NoContent>.Success(
                statusCode: StatusCodes.Status204NoContent
                 ));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAccountingUser()
        {
            ApplicationUser user = await userManager.FindByNameAsync(RoleInfo.Accounting);
            if (user.IsNull()) return CreateResponseInstance(Response<Guid>.Fail(
                 statusCode: StatusCodes.Status400BadRequest,
                 isShow: true,
                 path: "api/User/GetAccountinUser",
                 errors: "Gecerli bir kullanici bulunamadi"
                 ));


            return CreateResponseInstance(Response<Guid>.Success(
                 data:Guid.Parse(user.Id),
                 statusCode: StatusCodes.Status200OK
                  ));
        }
    }
}
