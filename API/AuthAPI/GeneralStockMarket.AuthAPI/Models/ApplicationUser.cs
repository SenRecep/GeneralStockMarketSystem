using Microsoft.AspNetCore.Identity;

namespace GeneralStockMarket.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string Address { get; set; }
    }
   
}
