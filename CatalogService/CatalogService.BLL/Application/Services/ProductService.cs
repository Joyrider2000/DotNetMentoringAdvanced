using CatalogService.BLL.Application.Repositories;
using CatalogService.BLL.Application.Services.Message;
using CatalogService.BLL.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL.Application.Services
{
    public class ProductService : BaseService<ProductEntity>, IProductService
    {
        private readonly IProductRepository _repository;

        private readonly IMessageService _messageService;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger, IMessageService messageService) : base(repository, logger)
        {
            _messageService = messageService;
        }

        public async Task<IEnumerable<ProductEntity>> GetByCategoryId(int categoryId)
        {
            return await _repository.GetByCategoryId(categoryId);
        }

        public async Task<IEnumerable<ProductEntity>> GetByCategoryId(int categoryId, int pageNumber, int pageSize)
        {
            return await _repository.GetByCategoryId(categoryId, pageNumber, pageSize);
        }

        public override async Task Publish(ProductEntity entity)
        {
            await _messageService.Publish(entity);
        }


    }
}
