using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Request;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface IRequestService
    {
        public Task<Response<RequestDto>> GetRequestsAsync();
        public Task<Response<NoContent>> PostRequestAsync(GenaralCreateDto dto);
    }
}
