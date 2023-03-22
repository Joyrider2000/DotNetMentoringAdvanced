using CartingService.DAL.Entities;

namespace CartingService.DAL.Repositories
{
    public interface ICartRepository
    {
        public IEnumerable<Cart> GetAllCarts();
        public IEnumerable<CartItem> GetCartItems(string cartId);
        public void AddCart(Cart cart);
        public void DeleteCart(string cartId);
        public void AddCartItem(string cartId, CartItem cartItem);
        public void DeleteCartItem(string cartId, int cartItemId);
    }
}
