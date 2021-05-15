using System;
using System.Threading.Tasks;

using GeneralStockMarket.DTO.Request;

namespace GeneralStockMarket.Bll.Interfaces
{
    public interface IRequestService
    {
        public Task<RequestDto> GetRequestsAsync(Guid id);
    }
}
