

using System.Collections.Generic;

using GeneralStockMarket.AuthAPI.Models;
using GeneralStockMarket.CoreLib.StringInfo;

using Microsoft.AspNetCore.Identity;

namespace GeneralStockMarket.AuthAPI.Seeding
{
    public static class DefaultUsersAndRoles
    {
        public static IEnumerable<SignUpViewModel> GetDevelopers()
        {
            yield return new SignUpViewModel()
            {
                UserName = "Daniga",
                Email = "me@senrecep.com",
                Password = "Password12*"
            };
            yield return new SignUpViewModel()
            {
                UserName = "Yusuf",
                Email = "yusuftopkaya@protonmail.com",
                Password = "123Password*"
            };
        }

        public static IEnumerable<IdentityRole> GetRoles()
        {
            yield return new IdentityRole(RoleInfo.Developer);
            yield return new IdentityRole(RoleInfo.Admin);
            yield return new IdentityRole(RoleInfo.Customer);
            yield return new IdentityRole(RoleInfo.IsVerified);
        }
    }
}
