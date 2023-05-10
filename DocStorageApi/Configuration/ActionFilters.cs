using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DocStorageApi.Configuration
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Executed before the action method
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null && context.ExceptionHandled == false)
            {
                context.Result = new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = 500
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
