using AutoMapper;
using CartingService.BLL.Entities;
using CartingService.CartingService.BLL.Entities.External.CatalogService;

namespace CartingService.CartingService.BLL.Mappers.Builders
{
    public static class ProductMapperBuilder
    {
        public static IMapper Build()
        {
            return new MapperConfiguration(config => {
                var map = config.CreateMap<ProductEntity, CartItemEntity>();
                map.ForMember("Image", expr => expr.MapFrom(item => new ImageEntity { URL = item.Image }));
            }).CreateMapper();
        }
    }
}
