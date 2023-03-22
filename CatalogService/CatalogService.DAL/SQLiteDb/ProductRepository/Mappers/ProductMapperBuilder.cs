using AutoMapper;
using CatalogService.DAL.SQLiteDb.Base.Mappers;
using CatalogService.DAL.SQLiteDb.CategoryRepository.Entities;
using CatalogService.DAL.SQLiteDb.ProductRepository.Entities;
using CatalogService.BLL.Domain.Entities;

namespace CatalogService.DAL.SQLiteDb.ProductRepository.Mappers
{
    public class ProductMapperBuilder : BaseMapperBuilder
    {
        public static IMapper Build()
        {
            var config = new MapperConfiguration(config =>
            {
                Dictionary<string, Action<IMemberConfigurationExpression<Product, ProductEntity, object>>> customMapFwd = new();
                Dictionary<string, Action<IMemberConfigurationExpression<ProductEntity, Product, object>>> customMapBwd = new();

                customMapBwd.Add("CategoryId", expr => expr.MapFrom(item => item.Category.Id));
                customMapBwd.Add("Category", expr => expr.MapFrom(item => (Category?)null));
                CreateTwoWayMap(config, customMapFwd, customMapBwd);
                CreateTwoWayMap<Category, CategoryEntity>(config);
            });
            return config.CreateMapper();
        }
    }
}
