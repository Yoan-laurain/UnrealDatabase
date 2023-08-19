using EF.Entities.Contexts;
using InternalApi.Module;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UnrealApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        ValidateLifetime = true,
        IssuerSigningKey = (SymmetricSecurityKey)new(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SigningKey"]!)),
        ValidateIssuerSigningKey = true
    };
});

string? connectionString = builder.Configuration.GetConnectionString("DataBaseConnection");

builder.Services.AddDbContext<EFContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("InternalApi"));
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.MapItemEndpoints();
app.MapAuthenticationEndpoints();

app.Run();