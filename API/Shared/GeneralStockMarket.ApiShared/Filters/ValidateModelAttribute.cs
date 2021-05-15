
using GeneralStockMarket.ApiShared.ExtensionMethods;

using Microsoft.AspNetCore.Mvc.Filters;

namespace GeneralStockMarket.ApiShared.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = context.GetBadRequestResultErrorDtoForModelState();
        }
    }
}
