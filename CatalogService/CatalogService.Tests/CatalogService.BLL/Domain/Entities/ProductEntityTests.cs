using CatalogService.BLL.Domain.Entities;

namespace CatalogService.Tests.CatalogService.BLL.Domain.Entities
{
    [TestClass]
    public class ProductEntityTests
    {
        [TestMethod]
        public void ValidProductEntityTest()
        {
            var productEntity = new ProductEntity { Id = 1, Name = "Name", Price = 11.11M, Amount = 2, Category = new CategoryEntity() };
            Assert.IsTrue(productEntity.isValid());
        }

        [TestMethod]
        public void MissingAmountProductEntityTest()
        {
            var productEntity = new ProductEntity { Id = 1, Name = "Name", Price = 11.11M, Category = new CategoryEntity() };
            Assert.IsFalse(productEntity.isValid());
        }
    }
}