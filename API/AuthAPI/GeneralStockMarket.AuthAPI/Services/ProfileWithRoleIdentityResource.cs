
using IdentityModel;

using IdentityServer4.Models;

namespace GeneralStockMarket.AuthAPI.Services
{
    public class ProfileWithRoleIdentityResource : IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}
