using CatalogService.BLL.Application.Repositories;
using CatalogService.BLL.Application.Services;
using CatalogService.BLL.Domain.Entities;
using Microsoft.Extensions.Logging;
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
            Mock<ILogger<BaseService<ProductEntity>>> mockLogger = new();
            mockRepository.Setup(x => x.List()).ReturnsAsync(new List<ProductEntity> { new ProductEntity() });
            var service = new BaseService<ProductEntity>(mockRepository.Object, mockLogger.Object);
            var result = service.List();
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Result.Count() == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void AddInvalidProductTest()
        {
            Mock<IBaseRepository<ProductEntity>> mockRepository = new();
            Mock<ILogger<BaseService<ProductEntity>>> mockLogger = new();
            var service = new BaseService<ProductEntity>(mockRepository.Object, mockLogger.Object);
            service.Add(new ProductEntity());
        }
    }
}
