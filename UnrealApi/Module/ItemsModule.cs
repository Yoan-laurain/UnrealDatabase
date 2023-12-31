﻿using EF.Entities.Contexts;
using InternalApi.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace UnrealApi.Models
{
    public static class ItemsModule
    {
        public static RouteGroupBuilder MapItemEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("");

            group.MapPost("/ObjectRecolted", async Task<Ok<bool>> ( EFContext db, ClaimsPrincipal user) =>
            {
                return TypedResults.Ok(true);
            }).AddEndpointFilter<ApiKeyAuthenticationEndPointFilter>();

            return group;
        }
    }
}
