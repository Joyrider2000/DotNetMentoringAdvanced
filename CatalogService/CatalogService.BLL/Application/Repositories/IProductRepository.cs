using CatalogService.BLL.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL.Application.Repositories
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
        public Task<IEnumerable<ProductEntity>> GetByCategoryId(int categoryId);
        public Task<IEnumerable<ProductEntity>> GetByCategoryId(int categoryId, int pageNumber, int pageSize);
    }
}
