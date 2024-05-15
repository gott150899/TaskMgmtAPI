using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation.WebAPI.Middlewares;

namespace TaskMgmtApi.Middlewares
{
    public class ReturnResponseAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            try
            {
                ObjectResult originResult = context.Result as ObjectResult;
                // Set header json
                if (originResult != null)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    int statusCode = originResult.StatusCode ?? StatusCodes.Status200OK;
                    // Rewrite response
                    context.Result = MiddlewareContext.ModifyContentResult(statusCode, originResult.Value);
                }

            }
            catch (InvalidCastException ex)
            {
                // Specific error for casting issues
                context.Result = new ContentResult()
                {
                    StatusCode = 400, // Bad Request
                    Content = "Invalid request format.",
                    ContentType = "application/json"
                };
            }
            catch (ArgumentNullException ex)
            {
                // Specific error for missing parameters
                context.Result = new ContentResult()
                {
                    StatusCode = 400, // Bad Request
                    Content = "Missing parameter: " + ex.ParamName,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                // General error handling for other exceptions
                context.Result = new ContentResult()
                {
                    StatusCode = 500, // Internal Server Error
                    Content = "An error occurred processing your request: " + ex.Message,
                    ContentType = "application/json"
                };
            }
        }
    }
}
