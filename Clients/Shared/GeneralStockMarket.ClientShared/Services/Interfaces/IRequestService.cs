using System;
using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.DTO.Request.Enums;

namespace GeneralStockMarket.ClientShared.Services.Interfaces
{
    public interface IRequestService
    {
        public Task<Response<RequestDto>> GetRequestsAsync();
        public Task<Response<RequestDto>> GetAllRequestsAsync();
        public Task<Response<NewTypeRequestDto>> GetNewTypeRequestAsync(Guid id);
        public Task<Response<NoContent>> PostRequestAsync(GenaralCreateDto dto);
        public Task<Response<NoContent>> DeleteRequestAsync(RequestType type,Guid id);
        public Task<Response<NoContent>> VerifyUpdateRequestAsync(bool mode,RequestType type,Guid id);
    }
}
