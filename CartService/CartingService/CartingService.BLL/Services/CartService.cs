using CartingService.DAL.Repositories;
using CartingService.DAL.Entities;
using CartingService.DAL.Exceptions;
using System.ComponentModel.DataAnnotations;
using CartingService.BLL.Entities;
using AutoMapper;
using CartingService.BLL.Mappers.Builders;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace CartingService.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartService> _logger;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
            _mapper = CartMapperBuilder.Build();
        }

        public IEnumerable<CartEntity> GetAllCarts()
        {
            _logger.LogInformation("Getting all carts");
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
                _logger.LogInformation($"Cart data is not valid: {JsonSerializer.Serialize(cart)}");
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
                _logger.LogInformation($"Cart item data is not valid: {JsonSerializer.Serialize(cartItem)}");
                throw;
            }
        }

        public void DeleteCart(string cartId)
        {
            _cartRepository.DeleteCart(cartId);
        }

        public void PrintCartItems(CartEntity cart)
        {
            _logger.LogInformation($"Printing list of items in cart #{cart.Id}");
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

        public void UpdateProducts(CartItemEntity? cartItem)
        {
            _logger.LogInformation("Updating cart entity");
            _cartRepository.UpdateProducts(_mapper.Map<CartItem>(cartItem));
        }
    }
}
