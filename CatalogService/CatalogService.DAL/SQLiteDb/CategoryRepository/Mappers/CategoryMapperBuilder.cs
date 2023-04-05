using AutoMapper;
using CatalogService.DAL.SQLiteDb.Base.Mappers;
using CatalogService.DAL.SQLiteDb.CategoryRepository.Entities;
using CatalogService.BLL.Domain.Entities;

namespace CatalogService.DAL.SQLiteDb.CategoryRepository.Mappers
{
    public class CategoryMapperBuilder : BaseMapperBuilder
    {
        public static IMapper Build()
        {
            var config = new MapperConfiguration(config =>
            {
                Dictionary<string, Action<IMemberConfigurationExpression<Category, CategoryEntity, object>>> customMapFwd = new();
                Dictionary<string, Action<IMemberConfigurationExpression<CategoryEntity, Category, object>>> customMapBwd = new();

                customMapBwd.Add("ParentId", expr => expr.MapFrom(item => item.Parent.Id));
                customMapBwd.Add("Parent", expr => expr.MapFrom(item => (Category?)null));
                CreateTwoWayMap(config, customMapFwd, customMapBwd);
            });
            return config.CreateMapper();
        }
    }
}
