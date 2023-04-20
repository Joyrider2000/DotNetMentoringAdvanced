using CatalogService.DAL.SQLiteDb.ProductRepository;
using CatalogService.DAL.SQLiteDb.CategoryRepository;
using CatalogService.BLL.Domain.Entities;
using CatalogService.BLL.Application.Services;

string sqlConnectionString = @"Data Source=./Resources/CatalogServiceData.db";

IProductService productService = new ProductService(new ProductRepository(sqlConnectionString), null);
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
categoryService.Print(categoryService.Get(category.Id.Value).Result);
productService.Print(productService.Get(product.Id.Value).Result);

category.Name += category.Name;
product.Name += product.Name;
productService.Update(product);
categoryService.Update(category);
categoryService.Print(await categoryService.List());
productService.Print(await productService.List());

productService.Delete(product.Id.Value);
categoryService.Delete(category.Id.Value);
categoryService.Delete(parentCategory.Id.Value);
categoryService.Print(await categoryService.List());
productService.Print(await productService.List());
