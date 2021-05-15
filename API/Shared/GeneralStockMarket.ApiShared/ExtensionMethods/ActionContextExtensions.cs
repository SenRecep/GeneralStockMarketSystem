using System.Collections.Generic;
using System.Linq;

using GeneralStockMarket.CoreLib.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneralStockMarket.ApiShared.ExtensionMethods
{
    public static class ActionContextExtensions
    {
        public static Response<NoContent> GetErrorDtoForModelState(this ActionContext context)
        {
            IEnumerable<string> errors = context.ModelState.Values
                   .Where(x => x.Errors?.Count > 0)
                   .SelectMany(x => x.Errors)
                   .Select(x => x.ErrorMessage);
            Response<NoContent> response = Response<NoContent>.Fail(
                statusCode: StatusCodes.Status400BadRequest,
                isShow: true,
                path: context.HttpContext.Request.Path,
                errors: errors.ToArray());
            return response;
        }
        public static BadRequestObjectResult GetBadRequestResultErrorDtoForModelState(this ActionContext context)
        {
            return new BadRequestObjectResult(context.GetErrorDtoForModelState());
        }
    }
}
