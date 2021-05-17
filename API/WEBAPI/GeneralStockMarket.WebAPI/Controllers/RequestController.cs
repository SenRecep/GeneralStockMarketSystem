using System;
using System.Threading.Tasks;

using AutoMapper;

using GeneralStockMarket.ApiShared.ControllerBases;
using GeneralStockMarket.ApiShared.Models;
using GeneralStockMarket.ApiShared.Services.Interfaces;
using GeneralStockMarket.Bll.Interfaces;
using GeneralStockMarket.Bll.StringInfos;
using GeneralStockMarket.Core.Entities.Abstract;
using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.CoreLib.StringInfo;
using GeneralStockMarket.DTO.General;
using GeneralStockMarket.DTO.Request;
using GeneralStockMarket.DTO.Request.Enums;
using GeneralStockMarket.Entities.Concrete;

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

        ///<summary>
        ///Kullanıcının isteklerini getirme.
        ///</summary>  
        ///<response code="200">Başarıyla geldi.</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Guid userId = Guid.Parse(sharedIdentityService.GetUserId);
            RequestDto model = await requestService.GetRequestsAsync(userId);
            return CreateResponseInstance(Response<RequestDto>.Success(model, StatusCodes.Status200OK));
        }

        ///<summary>
        ///Tüm kullanıcıların isteklerini getirme.
        ///</summary>  
        ///<response code="200">Başarıyla geldi.</response>
        [HttpGet("GetAll")]
        [Authorize(Roles = RoleInfo.DeveloperOrAdmin)]
        public async Task<IActionResult> GetAll()
        {
            RequestDto model = await requestService.GetAllRequestsAsync();
            return CreateResponseInstance(Response<RequestDto>.Success(model, StatusCodes.Status200OK));
        }

        ///<summary>
        ///Kullanıcının isteğinin oluşturulması.
        ///</summary>  
        ///<response code="201">Başarıyla oluşturuldu.</response>
        ///<response code="400">Desteklenmeyen istek tipi.</response>
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
                        entity.CreatedUserId = userId;
                        await genericDepositRequestService.Commit();
                        break;
                    case RequestType.Product:
                        entity = await genericProductRequestService.AddAsync(dto);
                        entity.CreatedUserId = userId;
                        await genericProductRequestService.Commit();
                        break;
                    case RequestType.NewType:
                        entity = await genericNewTypeRequestService.AddAsync(dto);
                        entity.CreatedUserId = userId;
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
        ///<summary>
        ///Kullanıcının tip ve Id bilgisi alınan isteğinin silinmesi.
        ///</summary>  
        ///<response code="204">Başarıyla silindi.</response>
        ///<response code="400">Desteklenmeyen istek tipi.</response>
        HttpDelete("{type}/{id:guid}")]
        public async Task<IActionResult> Delete(RequestType type, Guid id)
        {
            Guid userId = Guid.Parse(sharedIdentityService.GetUserId);
            Response<NoContent> response = null;
            try
            {
                DeleteDto deleteDto = new() { Id = id, UpdateUserId = userId };
                switch (type)
                {
                    case RequestType.Deposit:
                        await genericDepositRequestService.RemoveAsync(deleteDto);
                        await genericDepositRequestService.Commit();
                        break;
                    case RequestType.Product:
                        await genericProductRequestService.RemoveAsync(deleteDto);
                        await genericProductRequestService.Commit();
                        break;
                    case RequestType.NewType:
                        await genericNewTypeRequestService.RemoveAsync(deleteDto);
                        await genericNewTypeRequestService.Commit();
                        break;
                    default:
                        throw new CustomException();
                }
                response = Response<NoContent>.Success(StatusCodes.Status204NoContent);
            }
            catch
            {
                response = Response<NoContent>.Fail(
                    statusCode: StatusCodes.Status400BadRequest,
                    isShow: true,
                    path: "[DELETE] api/request",
                    errors: "Desteklenmeyen istek tipi"
                    );
            }
            return CreateResponseInstance(response);
        }
        ///<summary>
        ///Adminin Id bilgisi girilen istekleri cevaplaması.
        ///</summary>  
        ///<response code="204">İstek onaylandı.</response>
        ///<response code="400">İstek reddedildi.</response>
        [HttpDelete("VerifyUpdate/{verify}/{type}/{id:guid}")]
        [Authorize(Roles = RoleInfo.DeveloperOrAdmin)]
        public async Task<IActionResult> VerifyUpdate(bool verify, RequestType type, Guid id)
        {
            Guid userId = Guid.Parse(sharedIdentityService.GetUserId);
            Response<NoContent> response = null;
            try
            {
                VerifyDto verifyDto = new() { Id = id, UpdateUserId = userId, Verify = verify };
                switch (type)
                {
                    case RequestType.Deposit:
                        await genericDepositRequestService.UpdateAsync(verifyDto);
                        await genericDepositRequestService.Commit();
                        await requestService.DepositRequestVerifyConditionAsync(verifyDto);

                        break;
                    case RequestType.Product:
                        await genericProductRequestService.UpdateAsync(verifyDto);
                        await genericProductRequestService.Commit();
                        await requestService.ProductDepositRequestVerifyConditionAsync(verifyDto);
                        break;
                    case RequestType.NewType:
                        await genericNewTypeRequestService.UpdateAsync(verifyDto);
                        await genericNewTypeRequestService.Commit();
                        break;
                    default:
                        throw new CustomException();
                }
                response = Response<NoContent>.Success(StatusCodes.Status204NoContent);
            }
            catch
            {
                response = Response<NoContent>.Fail(
                    statusCode: StatusCodes.Status400BadRequest,
                    isShow: true,
                    path: "[DELETE] api/request",
                    errors: "Desteklenmeyen istek tipi"
                    );
            }
            return CreateResponseInstance(response);
        }

        [HttpGet("GetNewTypeRequest/{id:guid}")]
        [Authorize(Roles = RoleInfo.DeveloperOrAdmin)]
        public async Task<IActionResult> GetNewTypeRequest(Guid id)
        {
            var dto = await genericNewTypeRequestService.GetByIdAsync<NewTypeRequestDto>(id);
            if (dto.IsNull())
                return CreateResponseInstance(Response<NewTypeRequestDto>.Fail(
                    statusCode: StatusCodes.Status404NotFound,
                    isShow: true,
                    path: "api/request/GetNewTypeRequest/{id}",
                    errors: "İstenilen veri bulunamadı"
                    ));
            return CreateResponseInstance(Response<NewTypeRequestDto>.Success(dto, StatusCodes.Status200OK));
        }

    }
}
