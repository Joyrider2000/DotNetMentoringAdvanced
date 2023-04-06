using CartingService.DAL.Entities;

namespace CartingService.Tests.CartingService.DAL.Entities
{
    [TestClass]
    public class CartItemTests
    {
        [TestMethod]
        public void ValidCartItemTest()
        {
            var cartItem = new CartItem { Id = 1, Name = "Name", Price = 11.11M, Quantity = 2 };
            Assert.IsTrue(cartItem.isValid());
        }

        [TestMethod]
        public void NoIdCartItemTest()
        {
            var cartItem = new CartItem { Name = "Name", Price = 11.11M, Quantity = 2 };
            Assert.IsFalse(cartItem.isValid());
        }

        [TestMethod]
        public void NoNameCartItemTest()
        {
            var cartItem = new CartItem { Id = 1, Price = 11.11M, Quantity = 2 };
            Assert.IsFalse(cartItem.isValid());
        }

        [TestMethod]
        public void ZeroPriceCartItemTest()
        {
            var cartItem = new CartItem { Id = 1, Name = "Name", Price = 0, Quantity = 2 };
            Assert.IsFalse(cartItem.isValid());
        }

        [TestMethod]
        public void ZeroQuantityCartItemTest()
        {
            var cartItem = new CartItem { Id = 1, Name = "Name", Price = 11.11M, Quantity = 0 };
            Assert.IsFalse(cartItem.isValid());
        }
    }
}
