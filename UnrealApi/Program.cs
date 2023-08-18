using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnrealApi.Models;
using EF.Entities.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

builder.Services.AddDbContext<EFContext>(options =>
{
    options.UseSqlServer();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();