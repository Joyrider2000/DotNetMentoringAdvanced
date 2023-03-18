using CatalogService.BLL.Application.Repositories;
using CatalogService.BLL.Application.Services;
using CatalogService.BLL.Domain.Entities;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Tests.CatalogService.BLL.Application.Services
{
    [TestClass]
    public class BaseServiceTests
    {
        [TestMethod]
        public void GetProductListTest()
        {
            Mock<IBaseRepository<ProductEntity>> mockRepository = new();
            mockRepository.Setup(x => x.List()).Returns(new List<ProductEntity> { new ProductEntity() });
            var service = new BaseService<ProductEntity>(mockRepository.Object);
            var result = service.List();
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddInvalidProductTest()
        {
            Mock<IBaseRepository<ProductEntity>> mockRepository = new();
            var service = new BaseService<ProductEntity>(mockRepository.Object);
            service.Add(new ProductEntity());
        }
    }
}
