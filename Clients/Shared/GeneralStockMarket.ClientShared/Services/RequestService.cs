using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.ClientShared.Services.Interfaces;
using GeneralStockMarket.CoreLib.Response;
using GeneralStockMarket.DTO.Product;
using GeneralStockMarket.DTO.Request;

using Microsoft.AspNetCore.Http;

namespace GeneralStockMarket.ClientShared.Services
{
    public class RequestService : IRequestService
    {
        private readonly HttpClient httpClient;

        public RequestService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Response<RequestDto>> GetRequestsAsync()
        {
            //return await httpClient.GetFromJsonAsync<Response<RequestDto>>("api/request");
            return Response<RequestDto>.Success(new RequestDto()
            {
                DepositRequestDtos = new List<DepositRequestDto>()
            {
                new DepositRequestDto()
                {
                    Id=Guid.NewGuid(),
                    Amount=100,
                    Description="Param var benim",
                    Verify=false
                },
                 new DepositRequestDto()
                {
                     Id=Guid.NewGuid(),
                    Amount=200,
                    Description="Param var benim",
                    Verify=true
                },
                  new DepositRequestDto()
                {
                      Id=Guid.NewGuid(),
                    Amount=300,
                    Description="Param var benim",
                    Verify=true
                }
            },
                NewTypeRequestDtos = new List<NewTypeRequestDto>()
            {
                new NewTypeRequestDto()
                {
                    Id=Guid.NewGuid(),
                    Name="Lazanya",
                    Description="Param var benim",
                    Verify=false
                },
                 new NewTypeRequestDto()
                {
                     Id=Guid.NewGuid(),
                    Name="Pinokyo",
                    Description="Param var benim",
                    Verify=true
                },
                  new NewTypeRequestDto()
                {
                      Id=Guid.NewGuid(),
                    Name="Muz",
                    Description="Param var benim",
                    Verify=true
                }
            },
                ProductDepositRequestDtos = new List<ProductDepositRequestDto>()
            {
                new ProductDepositRequestDto()
                {
                    Id=Guid.NewGuid(),
                    Amount=300,
                    Description="Param var benim",
                    Verify=false,
                     Product=new ProductDto()
                     {
                         Id=Guid.NewGuid(),
                         Name="Urun 1"
                     }
                },
                 new ProductDepositRequestDto()
                {
                     Id=Guid.NewGuid(),
                    Amount=400,
                    Description="Param var benim",
                    Verify=true,
                     Product=new ProductDto()
                     {
                         Id=Guid.NewGuid(),
                         Name="Urun 2"
                     }
                },
                  new ProductDepositRequestDto()
                {
                      Id=Guid.NewGuid(),
                    Amount=500,
                    Description="Param var benim",
                    Verify=true,
                     Product=new ProductDto()
                     {
                         Id=Guid.NewGuid(),
                         Name="Urun 3"
                     }
                }
            }

            }, StatusCodes.Status200OK);
        }
    }
}
