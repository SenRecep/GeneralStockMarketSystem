using System.Collections.Generic;

namespace GeneralStockMarket.DTO.Request
{
    public class RequestDto
    {
        public IEnumerable<NewTypeRequestDto> NewTypeRequestDtos { get; set; }
        public IEnumerable<DepositRequestDto> DepositRequestDtos { get; set; }
        public IEnumerable<ProductDepositRequestDto> ProductDepositRequestDtos { get; set; }
    }
}
