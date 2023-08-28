using Microsoft.AspNetCore.Http;

namespace InternalApi.Authentication
{
    public class ApiKeyAuthenticationEndPointFilter : IEndpointFilter
    {
        private const string ApiKeyHeaderName = "X-Api-Key";
        
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            string? apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];
            
            if (!IsApiKeyValid(apiKey))
            {
                return Results.Unauthorized();
            }
            
            return await next(context);
        }

        private bool IsApiKeyValid(string? apiKey)
        {
            return !String.IsNullOrEmpty(apiKey);
        }
    }
}
