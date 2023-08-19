using EF.Entities.Contexts;
using InternalApi.Module;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UnrealApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setup =>
{
    OpenApiSecurityScheme? jwtSecurityScheme = new()
    {
        Description = "JWT Authorization header using the Bearer scheme.<br/>Example: \"Authorization: Bearer {token}\"",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Reference = new()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, new List<string>() } });

    setup.SwaggerGeneratorOptions.SwaggerDocs.Add("v1", new OpenApiInfo { Version = "v1", Title = "Internal API" });
    setup.SwaggerGeneratorOptions.SwaggerDocs.Add("updater", new OpenApiInfo { Version = "v1", Title = "Updater" });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        IssuerSigningKey = (SymmetricSecurityKey)new(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SigningKey"]!)),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

string? connectionString = builder.Configuration.GetConnectionString("DataBaseConnection");

builder.Services.AddDbContext<EFContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("InternalApi"));
});

builder.Services.AddAuthorization();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapItemEndpoints();
app.MapAuthenticationEndpoints();

app.Run();