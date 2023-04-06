using CartingService.BLL.Services;
using CartingService.DAL.Controller;
using CartingService.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

string connectionString = @"CartData.db";

ICartRepository cartRepository = new CartRepository(connectionString);
ICartService cartService = new CartService(cartRepository);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(service => cartService);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
