using CatalogService.DAL.SQLiteDb.ProductRepository;
using CatalogService.DAL.SQLiteDb.CategoryRepository;
using CatalogService.BLL.Domain.Entities;
using CatalogService.BLL.Application.Services;

string sqlConnectionString = @"Data Source=CatalogServiceData.db";

IBaseService<ProductEntity> productService = new BaseService<ProductEntity>(new ProductRepository(sqlConnectionString));
IBaseService<CategoryEntity> categoryService = new BaseService<CategoryEntity>(new CategoryRepository(sqlConnectionString));

CategoryEntity parentCategory = new()
{
    Name = "ParentCategory"
};


parentCategory.Id = categoryService.Add(parentCategory).Id;

CategoryEntity category = new()
{
    Image = "http://myimage.com",
    Name = "MyCategory",
    Parent = parentCategory,
};

category.Id = categoryService.Add(category).Id;

ProductEntity product = new()
{
    Category = category,
    Description = @"<html><body><a href=""foo.bar"" class=""blap"">blip</a></body></html>",
    Amount = 5,
    Price = 6.78M,
    Image = "http://myimage.com",
    Name = "MyProduct"
};

product.Id = productService.Add(product).Id;
categoryService.Print(categoryService.Get(category.Id));
productService.Print(productService.Get(product.Id));

category.Name += category.Name;
product.Name += product.Name;
productService.Update(product);
categoryService.Update(category);
categoryService.Print(categoryService.List());
productService.Print(productService.List());

productService.Delete(product.Id);
categoryService.Delete(category.Id);
categoryService.Delete(parentCategory.Id);
categoryService.Print(categoryService.List());
productService.Print(productService.List());
