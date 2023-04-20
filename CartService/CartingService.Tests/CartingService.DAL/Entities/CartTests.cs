using CartingService.DAL.Entities;

namespace CartingService.Tests.CartingService.DAL.Entities
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void ValidCartTest()
        {
            var cart = new Cart { Id = "Id" , Items = new()};
            Assert.IsTrue(cart.isValid());
        }

        [TestMethod]
        public void NoIdCartTest()
        {
            var cart = new Cart { Items = new() };
            Assert.IsFalse(cart.isValid());
        }

        [TestMethod]
        public void NoItemsCartTest()
        {
            var cart = new Cart { Id = "Id" };
            Assert.IsFalse(cart.isValid());
        }
    }
}
