using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Request;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneralStockMarket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : CustomControllerBase
    {
        private readonly IRequestService requestService;
        private readonly ISharedIdentityService sharedIdentityService;

        public RequestController(IRequestService requestService,
            ISharedIdentityService sharedIdentityService)
        {
            this.requestService = requestService;
            this.sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(sharedIdentityService.GetUserId);
            var model = await requestService.GetRequestsAsync(userId);
            return CreateResponseInstance(Response<RequestDto>.Success(model,StatusCodes.Status200OK));
        }
        //[HttpPost]
        //public async Task<IActionResult> Post(GenaralCreateDto model)
        //{

        //}
    }
}
