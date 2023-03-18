using CartingService.BLL.Entities;
using CartingService.DAL.Entities;

namespace CartingService.BLL.Services
{
    public interface ICartService
    {
        public IEnumerable<CartEntity> GetAllCarts();
        public IEnumerable<CartItemEntity> GetCartItems(string cartId);
        public void AddCart(CartEntity cart);
        public void DeleteCart(string cartId);
        public void AddCartItem(string cartId, CartItemEntity cartItem);
        public void DeleteCartItem(string cartId, int cartItemId);
        public void PrintCartItems(CartEntity cart);
        public void PrintAllCarts();
    }
}
