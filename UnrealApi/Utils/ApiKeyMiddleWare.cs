using EF.Entities.Contexts;

namespace InternalApi.Utils
{
    public class ApiKeyMiddleWare
    {
        private readonly RequestDelegate _next;
        const string APIKEY = "ApiKey";

        public ApiKeyMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, EFContext db)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out
                    var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided ");
                return;
            }

            string? apiKey = db.Players.FirstOrDefault(u => u.ApiKey == extractedApiKey.ToString())?.ApiKey;
            
            if ( apiKey is null )
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
            }

            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Authorized client.");
  
            await _next(context);
        }
    }
}
