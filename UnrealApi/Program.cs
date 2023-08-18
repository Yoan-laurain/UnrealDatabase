using EF.Entities.Contexts;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using UnrealApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapItemEndpoints();

app.Run();