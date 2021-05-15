
using GeneralStockMarket.CoreLib.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneralStockMarket.ApiShared.ControllerBases
{
    public class CustomControllerBase : ControllerBase
    {
        public IActionResult CreateResponseInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode == StatusCodes.Status204NoContent? StatusCodes.Status200OK:response.StatusCode
            };
        }
    }
}
