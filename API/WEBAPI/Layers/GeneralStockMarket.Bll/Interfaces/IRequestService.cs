using System;
using System.Threading.Tasks;

using GeneralStockMarket.DTO.Request;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface IRequestService
    {
        public Task<RequestDto> GetRequestsAsync(Guid id);
        public  Task<RequestDto> GetAllRequestsAsync();
        public  Task DepositRequestVerifyConditionAsync(VerifyDto dto);
        public  Task ProductDepositRequestVerifyConditionAsync(VerifyDto dto);
    }
}
