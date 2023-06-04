using CartingService.BLL.Services;
using CartingService.DAL.Entities;
using CartingService.DAL.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace CartingService.Tests.CartingService.BLL.Services
{
    [TestClass]
    public class CartServiceTests
    {

        private readonly Cart cart = new();

        [TestInitialize]
        public void Init()
        {
            cart.Id = "CartId";
            cart.Items = new List<CartItem>();
        }

        [TestMethod]
        public void GetAllCartsTest()
        {
            Mock<ICartRepository> mockRepository = new();
            Mock<ILogger<CartService>> mockLogger = new();
            mockRepository.Setup(x => x.GetAllCarts()).Returns(new List<Cart>{ cart });
            var service = new CartService(mockRepository.Object, mockLogger.Object);

            var result = service.GetAllCarts();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 1);
            Assert.IsTrue(JsonSerializer.Serialize(result.First()).Equals(JsonSerializer.Serialize(cart)));
        }
    }
}
