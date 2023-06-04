﻿using CatalogService.BLL.Application.Repositories;
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
        public async Task GetProductListTest()
        {
            Mock<IBaseRepository<ProductEntity>> mockRepository = new();
            Mock<ILogger<BaseService<ProductEntity>>> mockLogger = new();
            mockRepository.Setup(x => x.List()).ReturnsAsync(new List<ProductEntity> { new ProductEntity() });
            var service = new BaseService<ProductEntity>(mockRepository.Object, mockLogger.Object);
            var result = await service.List();
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task AddInvalidProductTest()
        {
            Mock<IBaseRepository<ProductEntity>> mockRepository = new();
            Mock<ILogger<BaseService<ProductEntity>>> mockLogger = new();
            var service = new BaseService<ProductEntity>(mockRepository.Object, mockLogger.Object);
            await service.Add(new ProductEntity());
        }
    }
}
