using System;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Models;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.StringInfos;
using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.DTO.Request.Enums;
using GeneralStockMarket.Entities.Concrete;

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
        private readonly IMapper mapper;

        private readonly IGenericService<ProductDepositRequest> genericProductRequestService;
        private readonly IGenericService<NewTypeRequest> genericNewTypeRequestService;
        private readonly IGenericService<DepositRequest> genericDepositRequestService;

        public RequestController(
            IRequestService requestService,
            ISharedIdentityService sharedIdentityService,
            IMapper mapper,
            IGenericService<ProductDepositRequest> genericProductRequestService,
            IGenericService<NewTypeRequest> genericNewTypeRequestService,
            IGenericService<DepositRequest> genericDepositRequestService)
        {
            this.requestService = requestService;
            this.sharedIdentityService = sharedIdentityService;
            this.mapper = mapper;

            this.genericProductRequestService = genericProductRequestService;
            this.genericNewTypeRequestService = genericNewTypeRequestService;
            this.genericDepositRequestService = genericDepositRequestService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Guid userId = Guid.Parse(sharedIdentityService.GetUserId);
            RequestDto model = await requestService.GetRequestsAsync(userId);
            return CreateResponseInstance(Response<RequestDto>.Success(model, StatusCodes.Status200OK));
        }
        [HttpPost]
        public async Task<IActionResult> Post(GenaralCreateDto model)
        {
            Guid userId = Guid.Parse(sharedIdentityService.GetUserId);
            model.UserId = userId;
            Response<NoContent> response = null;
            try
            {
                IDTO dto = model.RequestType switch
                {
                    RequestType.Deposit => mapper.Map<CreateDepositRequest>(model),
                    RequestType.Product => mapper.Map<CreateProductDepositRequest>(model),
                    RequestType.NewType => mapper.Map<CreateNewTypeRequest>(model),
                    _ => throw new CustomException()
                };
                IEntityBase entity = null;
                switch (model.RequestType)
                {
                    case RequestType.Deposit:
                        entity = await genericDepositRequestService.AddAsync(dto);
                        entity.CreatedUserId = Guid.Parse(UserStringInfo.SystemUserId);
                        await genericDepositRequestService.Commit();
                        break;
                    case RequestType.Product:
                        entity = await genericProductRequestService.AddAsync(dto);
                        entity.CreatedUserId = Guid.Parse(UserStringInfo.SystemUserId);
                        await genericProductRequestService.Commit();
                        break;
                    case RequestType.NewType:
                        entity = await genericNewTypeRequestService.AddAsync(dto);
                        entity.CreatedUserId = Guid.Parse(UserStringInfo.SystemUserId);
                        await genericNewTypeRequestService.Commit();
                        break;
                    default:
                        throw new CustomException();
                }
                response = Response<NoContent>.Success(StatusCodes.Status201Created);
            }
            catch
            {
                response = Response<NoContent>.Fail(
                    statusCode: StatusCodes.Status400BadRequest,
                    isShow: true,
                    path: "[POST] api/request",
                    errors: "Desteklenmeyen istek tipi"
                    );
            }
            return CreateResponseInstance(response);

        }
    }
}
