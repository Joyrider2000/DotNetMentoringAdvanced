using AutoMapper;
using CartingService.BLL.Entities;
using CartingService.DAL.Entities;

namespace CartingService.BLL.Mappers.Builders
{
    internal class CartMapperBuilder : BaseMapperBuilder
    {
        public static IMapper Build()
        {
            var config = new MapperConfiguration(config =>
            {
                CreateTwoWayMap<Cart, CartEntity>(config);
                CreateTwoWayMap<CartItem, CartItemEntity>(config);
                CreateTwoWayMap<Image, ImageEntity>(config);
            });
            return config.CreateMapper();
        }
    }
}
