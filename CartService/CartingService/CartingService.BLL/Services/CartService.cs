using CartingService.DAL.Repositories;
using CartingService.DAL.Entities;
using CartingService.DAL.Exceptions;
using System.ComponentModel.DataAnnotations;
using CartingService.BLL.Entities;
using AutoMapper;
using CartingService.BLL.Mappers.Builders;
using System.Text.Json;

namespace CartingService.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _mapper = CartMapperBuilder.Build();
        }

        public IEnumerable<CartEntity> GetAllCarts()
        {
            return _mapper.Map<IEnumerable<CartEntity>>(_cartRepository.GetAllCarts());
        }

        public CartEntity GetCart(string cartId)
        {
            return _mapper.Map<CartEntity>(_cartRepository.GetCart(cartId));
        }

        public CartEntity AddCart(CartEntity cart)
        {
            try
            {
                return _mapper.Map<CartEntity>(_cartRepository.AddCart(_mapper.Map<Cart>(cart)));
            }
            catch (ValidationException)
            {
                Console.WriteLine($"Cart data is not valid: {JsonSerializer.Serialize(cart)}");
                throw;
            }
        }

        public IEnumerable<CartItemEntity> GetCartItems(string cartId)
        {
            try
            {
                return _mapper.Map<IEnumerable<CartItemEntity>>(_cartRepository.GetCartItems(cartId));
            }
            catch (KeyNotFoundException)
            {
                return Enumerable.Empty<CartItemEntity>();
            }
            catch (PropertyNotFoundException)
            {
                return Enumerable.Empty<CartItemEntity>();
            }
        }

        public void DeleteCartItem(string cartId, int cartItemId)
        {
            _cartRepository.DeleteCartItem(cartId, cartItemId);
        }

        public void AddCartItem(string cartId, CartItemEntity cartItem)
        {
            try
            {
                if (_cartRepository.GetCart(cartId) == null)
                {
                    _cartRepository.AddCart(new Cart(cartId));
                }
                _cartRepository.AddCartItem(cartId, _mapper.Map<CartItem>(cartItem));
            }
            catch (ItemExistsException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            catch (ValidationException)
            {
                Console.WriteLine($"Cart item data is not valid: {JsonSerializer.Serialize(cartItem)}");
                throw;
            }
        }

        public void DeleteCart(string cartId)
        {
            _cartRepository.DeleteCart(cartId);
        }

        public void PrintCartItems(CartEntity cart)
        {
            Console.WriteLine($"Printing list of items in cart #{cart.Id}");
            if (cart.Items != null)
            {
                Console.WriteLine(JsonSerializer.Serialize(cart.Items));
            }
        }

        public void PrintAllCarts()
        {
            foreach (CartEntity cart in GetAllCarts())
            {
                PrintCartItems(cart);
            }
        }
    }
}
