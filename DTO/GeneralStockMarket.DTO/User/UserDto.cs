
using GeneralStockMarket.CoreLib.Interfaces;

namespace GeneralStockMarket.DTO.User
{
    public class UserDto : IDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
