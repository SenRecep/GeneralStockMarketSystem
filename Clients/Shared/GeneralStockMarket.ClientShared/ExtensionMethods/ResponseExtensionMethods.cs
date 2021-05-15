using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using GeneralStockMarket.CoreLib.ExtensionMethods;
using GeneralStockMarket.CoreLib.Response;

using IdentityModel.Client;

namespace GeneralStockMarket.ClientShared.ExtensionMethods
{
    public static class ResponseExtensionMethods
    {
        public static async Task<Response<T>> GetResponseAsync<T>(this ProtocolResponse protocolResponse, bool isShow, string path, string error)
        {
            Response<T> response = await protocolResponse.HttpResponse.Content.ReadFromJsonAsync<Response<T>>();
            if (response.ErrorData.IsNull())
            {
                response = Response<T>.Fail(
                    statusCode: (int)protocolResponse.HttpStatusCode,
                    isShow: isShow,
                    path: path,
                    errors: new[] {
                            protocolResponse.Error,
                            error
                    }
                    );
            }
            return response;
        }

        public static async Task<Response<T>> GetResponseAsync<T>(this HttpResponseMessage httpResponseMessage, bool isShow, string path, string error)
        {
            Response<T> response = await httpResponseMessage.Content.ReadFromJsonAsync<Response<T>>();
            if (response == null || response?.ErrorData.Errors?.Count() == 0)
            {
                response = Response<T>.Fail(
                    statusCode: (int)httpResponseMessage.StatusCode,
                    isShow: isShow,
                    path: path,
                    errors: error
                    );
            }
            return response;
        }
    }
}
