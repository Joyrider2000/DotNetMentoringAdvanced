using CatalogService.BLL.Application.Repositories;
using CatalogService.BLL.Domain.Entities;
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

        public ProductService(IProductRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductEntity>> GetByCategoryId(int categoryId)
        {
            return await _repository.GetByCategoryId(categoryId);
        }

        public async Task<IEnumerable<ProductEntity>> GetByCategoryId(int categoryId, int pageNumber, int pageSize)
        {
            return await _repository.GetByCategoryId(categoryId, pageNumber, pageSize);
        }
    }
}
