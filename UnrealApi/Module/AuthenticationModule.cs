using EF.Entities.Contexts;
using EF.Entities.Models;
using InternalApi.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EF.Entities.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace InternalApi.Module
{
    public static class AuthenticationModule
    {
        public static RouteGroupBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("");
            group.WithTags("Authentication");
            group.WithGroupName("v1");

            group.MapPost("/RegisterUser", [AllowAnonymous] async Task<Results<Ok<string>, BadRequest<string>>> (EFContext db, IConfiguration configuration, UserViewModel player) =>
            {
                if (!new EmailAddressAttribute().IsValid(player.Email))
                {
                    return TypedResults.BadRequest("Email format is invalid");
                }

                if (!Regex.IsMatch(player.Password, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
                {
                    return TypedResults.BadRequest("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character");
                }

                if (await db.Players.Where(x => x.Email == player.Email).AnyAsync())
                {
                    return TypedResults.BadRequest("Email already exists");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(player.Password);

                Player player1 = new Player();

                player1.Password = passwordHash;
                player1.Email = player.Email.ToLower();
                player1.Name = "";
                player1.Items = new List<Item>();

                string AccessToken = CreateSecurityToken(configuration, new List<Claim> { });

                await db.Players.AddAsync(player1);
                await db.SaveChangesAsync();
                
                return TypedResults.Ok(AccessToken);
            }); 

            group.MapPost("/Authenticate", [AllowAnonymous] async Task<Results<Ok<UserTokenViewModel>, BadRequest<string>>> (EFContext db, IConfiguration configuration, UserViewModel player) =>
            {
                if (await IsLoginAttemptAuthorized(db, player.Email))
                {
                    Player? user = await db.Players.Where(x => x.Email == player.Email).FirstOrDefaultAsync();

                    // Email not found
                    if ( user is null )
                    {
                        return TypedResults.BadRequest("WrongCredentials");
                    }

                    // Password is wrong
                    if ( !BCrypt.Net.BCrypt.Verify(player.Password,user.Password) )
                    {
                        return TypedResults.BadRequest("WrongCredentials");
                    }

                    var claims = new List<Claim>
                            {
                                new Claim("UserId", user.Id.ToString()),
                            };

                    string token = CreateSecurityToken(configuration, claims);

                    UserTokenViewModel userTokenViewModel = new UserTokenViewModel();
                    userTokenViewModel.AccessToken = token;
                    userTokenViewModel.User = user;

                    return TypedResults.Ok(userTokenViewModel);
                }
                else
                {
                    return TypedResults.BadRequest("TooManyAttempts");
                }
            });

            return group;
        }

        private static async Task<bool> IsLoginAttemptAuthorized(EFContext db, string email)
        {
            const int FailuresBeforeRestriction = 10;
            const int MaxFailuresInterval = 2;
            const int RestrictionDuration = 10;

            DateTime oldestRestrictionPossible = DateTime.UtcNow.AddMinutes(-RestrictionDuration);
            DateTime oldestConcernedFailurePossible = oldestRestrictionPossible.AddMinutes(-MaxFailuresInterval * FailuresBeforeRestriction);

            List<LoginAttempt> concernedLoginAttempts = await db.LoginAttempts
                .Where(la => la.Email == email 
                && la.Date >= oldestConcernedFailurePossible 
                && la.LoginResultId == (int)LoginResultEnum.WrongCredentials)
                .OrderByDescending(la => la.Date)
                .ToListAsync();

            List<LoginAttempt> consecutiveFailures = new();
            foreach (LoginAttempt loginFailure in concernedLoginAttempts)
            {
                // if the failure is too old, clear the consecutive failures list
                if (consecutiveFailures.Count != 0 && (consecutiveFailures.Last().Date - loginFailure.Date).TotalMinutes > MaxFailuresInterval)
                {
                    consecutiveFailures.Clear();
                }

                // Check if a failure outside the restriction period occurred
                if (consecutiveFailures.Count == 0 && loginFailure.Date < oldestRestrictionPossible)
                {
                    return true; 
                }

                consecutiveFailures.Add(loginFailure);

                // Check if consecutive failure threshold reached
                if (consecutiveFailures.Count >= FailuresBeforeRestriction)
                {
                    return false;
                }
            }

            return true;
        }

        private static string CreateSecurityToken(IConfiguration configuration, List<Claim> claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SigningKey"]!));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(jwtToken);
        }
    }
}