using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.AuthAPI.ExtensionMethods;
using GeneralStockMarket.AuthAPI.Models;
using GeneralStockMarket.CoreLib.StringInfo;

using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace GeneralStockMarket.AuthAPI.Seeding
{
    public static class IdentityServerSeedData
    {

        public static async Task SeedUserData(IServiceProvider serviceProvider)
        {
            await SeedRoles(serviceProvider);
            await SeedUsers(serviceProvider);
            await AcountingUser(serviceProvider);
        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (IdentityRole role in DefaultUsersAndRoles.GetRoles())
            {
                IdentityRole found = await roleManager.FindByNameAsync(role.Name);
                if (found != null) continue;

                IdentityResult result = await roleManager.CreateAsync(role);
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
            }
        }



        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (SignUpViewModel model in DefaultUsersAndRoles.GetDevelopers())
            {
                ApplicationUser found = await userManager.FindByNameAsync(model.UserName);
                if (found != null) continue;

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                result = await userManager.AddToRoleAsync(user, RoleInfo.Developer);

                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
            }
        }

        private static async Task AcountingUser(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var model = new SignUpViewModel()
            {
                UserName = RoleInfo.Accounting,
                Email = "accounting@generalstockmarket.com",
                Password = "Password12*"
            };
            ApplicationUser found = await userManager.FindByNameAsync(model.UserName);
            if (found != null) return;

            ApplicationUser user = new ApplicationUser()
            {
                UserName = model.UserName,
                Address = "General Stock Market",
                FirstName = "General Stock Market",
                LastName = model.UserName,
                Email=model.Email,
                IdentityNumber = "1111111111"
            };



            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

             result = await userManager.SetPhoneNumberAsync(user, "05319649002");
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

            result = await userManager.AddToRoleAsync(user, RoleInfo.Accounting);
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
            result = await userManager.AddToRoleAsync(user, RoleInfo.IsVerified);
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
        }

        public static async Task SeedConfiguration(ConfigurationDbContext context)
        {
            List<Task> tasks = new List<Task>();

            if (context.Clients.Count() != Config.Clients.Count())
            {
                await context.Clients.Clear();
                tasks.Add(context.Clients.AddRangeAsync(Config.Clients.Select(x => x.ToEntity())));
            }
            if (context.ApiResources.Count() != Config.ApiResources.Count())
            {
                await context.ApiResources.Clear();
                tasks.Add(context.ApiResources.AddRangeAsync(Config.ApiResources.Select(x => x.ToEntity())));
            }

            if (context.ApiScopes.Count() != Config.ApiScopes.Count())
            {
                await context.ApiScopes.Clear();
                tasks.Add(context.ApiScopes.AddRangeAsync(Config.ApiScopes.Select(x => x.ToEntity())));
            }

            if (context.IdentityResources.Count() != Config.IdentityResources.Count())
            {
                await context.IdentityResources.Clear();
                tasks.Add(context.IdentityResources.AddRangeAsync(Config.IdentityResources.Select(x => x.ToEntity())));
            }

            await Task.WhenAll(tasks);
            await context.SaveChangesAsync();
        }
    }
}
