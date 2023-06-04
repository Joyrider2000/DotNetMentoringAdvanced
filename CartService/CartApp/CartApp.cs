using CartingService.BLL.Entities;
using CartingService.BLL.Services;
using CartingService.DAL.Controller;
using CartingService.DAL.Repositories;
using System.Text.Json;

string connectionString = @"CartData.db";

ICartRepository cartRepository = new CartRepository(connectionString);
ICartService cartService = new CartService(cartRepository);

CartEntity myCart = new CartEntity();
IList<CartItemEntity> cartItems = new List<CartItemEntity>();

myCart.Id = "MyCartId27";
cartItems.Add(new CartItemEntity { Id = 101, Name = "Name1", Price = 11.11M, Quantity = 2, Image = new ImageEntity { URL = "http://myUrl", AltText = "MyImage" } });
cartService.AddCart(new CartEntity { Id = "MyCartId25", Items = cartItems });
cartItems.Add(new CartItemEntity { Id = 102, Name = "Name1", Price = 22.22M, Quantity = 4, Image = new ImageEntity { URL = "http://myUrl2", AltText = "MyImage2" } });
myCart.Items = cartItems;


Console.WriteLine($"Creating cart #{myCart.Id}\n");
cartService.AddCart(myCart);
cartService.PrintAllCarts();

Console.WriteLine("\nAdding item\n");
cartService.AddCartItem(myCart.Id, new CartItemEntity { Id = 103, Name = "Name3" });
cartService.PrintAllCarts();

Console.WriteLine("\nListing items\n");
IEnumerable<CartItemEntity> newCartItems = cartService.GetCartItems("MyCartId27");
Console.WriteLine(string.Join(',', JsonSerializer.Serialize(newCartItems)));

Console.WriteLine("\nRemoving item\n");
cartService.DeleteCartItem(myCart.Id, 102);
cartService.PrintAllCarts();

Console.WriteLine($"\nDeleting cart #{myCart.Id}\n");
cartService.DeleteCart(myCart.Id);
cartService.PrintAllCarts();
