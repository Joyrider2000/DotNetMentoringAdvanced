using CartingService.BLL.Entities;
using CartingService.DAL.Entities;

namespace CartingService.DAL.Repositories
{
    public interface ICartRepository
    {
        public IEnumerable<Cart> GetAllCarts();
        public Cart GetCart(string cartId);
        public IEnumerable<CartItem> GetCartItems(string cartId);
        public Cart AddCart(Cart cart);
        public void DeleteCart(string cartId);
        public void AddCartItem(string cartId, CartItem cartItem);
        public void DeleteCartItem(string cartId, int cartItemId);
        public void UpdateProducts(CartItem cartItem);
    }
}
