using CatalogService.BLL.Application.Services;
using CatalogService.BLL.Domain.Entities;
using CatalogService.DAL.SQLiteDb.CategoryRepository;
using CatalogService.DAL.SQLiteDb.ProductRepository;
using Microsoft.AspNetCore.Mvc;

string sqlConnectionString = @"Data Source=CatalogServiceData.db"; 

IProductService productService = new ProductService(new ProductRepository(sqlConnectionString));
IBaseService<CategoryEntity> categoryService = new BaseService<CategoryEntity>(new CategoryRepository(sqlConnectionString));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient(service => productService);
builder.Services.AddTransient(service => categoryService);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
