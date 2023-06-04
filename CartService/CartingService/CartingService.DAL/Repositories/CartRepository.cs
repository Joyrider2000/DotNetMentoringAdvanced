using CartingService.DAL.Repositories;
using CartingService.DAL.Entities;
using LiteDB;
using CartingService.DAL.Exceptions;
using System.ComponentModel.DataAnnotations;
using CartingService.DAL.Configuration.Options;

namespace CartingService.DAL.Controller
{
    public class CartRepository : ICartRepository
    {
        private readonly AppOptions _appOptions;

        public CartRepository(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        private ILiteDatabase GetDatabase()
        {
            return new LiteDatabase(_appOptions.DbConnectionString);
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            using (var db = GetDatabase())
            {
                var col = db.GetCollection<Cart>("cart");
                return col.Query().ToList();
            }
        }

        public Cart GetCart(string cartId)
        {
            using (var db = GetDatabase())
            {
                var col = db.GetCollection<Cart>("cart");
                return col.Query().Where(c => c.Id == cartId).FirstOrDefault();
            }
        }

        public IEnumerable<CartItem> GetCartItems(string cartId)
        {
            using (var db = GetDatabase())
            {
                var col = db.GetCollection<Cart>("cart");
                Cart cart = col.Query().Where(c => c.Id == cartId).FirstOrDefault();
                if (cart == null)
                {
                    throw new KeyNotFoundException();
                }
                if (cart.Items == null)
                {
                    throw new PropertyNotFoundException();
                }
                return cart.Items.ToList();
            }
        }

        public Cart AddCart(Cart cart)
        {
            if (cart.isValid())
            {
                using (var db = GetDatabase())
                {
                    var col = db.GetCollection<Cart>("cart");

                    var mapper = BsonMapper.Global;
                    mapper.Entity<Cart>().Field(x => x.Items, "Items");

                    col.Upsert(cart);
                    return cart;
                }
            }
            else
            {
                throw new ValidationException();
            }
        }

        public void DeleteCartItem(string cartId, int cartItemId)
        {
            using (var db = GetDatabase())
            {
                var col = db.GetCollection<Cart>("cart");
                var cart = col.FindOne(c => c.Id == cartId);
                if ((cart == null) || (cart.Items == null) || (!cart.Items.Any(item => item.Id == cartItemId)))
                {
                    throw new KeyNotFoundException();
                }
                cart.Items.RemoveAll(item => item.Id == cartItemId);
                col.Update(cart);
            }
        }

        public void AddCartItem(string cartId, CartItem cartItem)
        {
            if (cartItem.isValid())
            {
                using (var db = GetDatabase())
                {
                    var col = db.GetCollection<Cart>("cart");
                    var cart = col.FindOne(c => c.Id == cartId);
                    if (cart != null && cart.Items != null)
                    {
                        if (cart.Items.Find(item => item.Id == cartItem.Id) != null)
                        {
                            throw new ItemExistsException();
                        }
                        else
                        {
                            cart.Items.Add(cartItem);
                        }
                        col.Update(cart);
                    }
                }
            }
            else
            {
                throw new ValidationException("Item data is not valid.");
            }
        }

        public void DeleteCart(string cartId)
        {
            using (var db = GetDatabase())
            {
                var col = db.GetCollection<Cart>("cart");
                col.Delete(cartId);
            }
        }

        public void UpdateProducts(CartItem cartItem)
        {
            using (var db = GetDatabase())
            {
                var col = db.GetCollection<Cart>("cart");
                foreach (Cart cart in col.FindAll())
                {
                    if (cart.Items != null)
                    {
                        CartItem? item = cart.Items.Find(item => item.Id == cartItem.Id);
                        if (item != null)
                        {
                            item.Price = cartItem.Price;
                            item.Name = cartItem.Name;
                            item.Image = new Image
                            {
                                URL = cartItem.Image?.URL,
                                AltText = cartItem.Image?.AltText ?? item.Image?.AltText
                            };
                            col.Update(cart);
                        }
                    }
                }
            }
        }
    }
}
