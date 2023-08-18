using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using EF.Entities.Context;

namespace UnrealApi.Models
{
    public static class ItemsModule
    {
        public static RouteGroupBuilder MapItemEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("");
            group.RequireAuthorization();

            group.MapPost("/ObjectRecolted", async Task<Ok<bool>> ( EFContext db, ClaimsPrincipal user) =>
            {
                return TypedResults.Ok(true);
            }).Produces<bool>();

            return group;
        }
    }
}
