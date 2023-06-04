using CartingService.BLL.Entities;
using CartingService.DAL.Entities;

namespace CartingService.BLL.Services
{
    public interface ICartService
    {
        public IEnumerable<CartEntity> GetAllCarts();
        public CartEntity GetCart(string cartId);
        public IEnumerable<CartItemEntity> GetCartItems(string cartId);
        public CartEntity AddCart(CartEntity cart);
        public void DeleteCart(string cartId);
        public void AddCartItem(string cartId, CartItemEntity cartItem);
        public void DeleteCartItem(string cartId, int cartItemId);
        public void PrintCartItems(CartEntity cart);
        public void PrintAllCarts();
        public void UpdateProducts(CartItemEntity? cartItem);
    }
}
