using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

public class ApiKeyAuthAttribute : ActionFilterAttribute
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

        if (configuration == null)
        {
            context.Result = new ContentResult
            {
                StatusCode = 500,
                Content = "Configuration service not available."
            };
            return;
        }

        var allowedApiKey = configuration.GetValue<string>("AllowedApiKey");

        if (string.IsNullOrEmpty(allowedApiKey))
        {
            context.Result = new ContentResult
            {
                StatusCode = 500,
                Content = "API Key configuration is missing."
            };
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "API Key was not provided."
            };
            return;
        }

        if (!allowedApiKey.Equals(extractedApiKey))
        {
            context.Result = new ContentResult
            {
                StatusCode = 403,
                Content = "Unauthorized client."
            };
            return;
        }

        base.OnActionExecuting(context);
    }
}
