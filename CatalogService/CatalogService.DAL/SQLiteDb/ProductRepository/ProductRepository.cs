using CatalogService.DAL.SQLiteDb.Base.Repositories;
using CatalogService.DAL.SQLiteDb.ProductRepository.Entities;
using CatalogService.DAL.SQLiteDb.ProductRepository.Mappers;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.Domain.Entities;
using CatalogService.BLL.Application.Repositories;
using CatalogService.DAL.Configuration.Options;
using Microsoft.Extensions.Logging;

namespace CatalogService.DAL.SQLiteDb.ProductRepository
{
    public class ProductRepository : BaseRepository<Product, ProductEntity>, IProductRepository
    {
        public ProductRepository(AppOptions appOptions, ILogger<ProductRepository> logger) : base(appOptions, ProductMapperBuilder.Build(), logger) { }

        protected override async Task<ProductEntity> GetById(DbContext db, int id)
        {
            return _mapper.Map<ProductEntity>(await db.Set<Product>().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id));
        }

        protected override async Task<IEnumerable<Product>> GetAll(DbContext db)
        {
            var res = await db.Set<Product>().Include(p => p.Category).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<ProductEntity>> GetByCategoryId(int categoryId)
        {
            using (var db = GetContext())
            {
                return _mapper.Map<List<ProductEntity>>(await db.Set<Product>()
                    .Where(p => p.CategoryId == categoryId)
                    .Include(p => p.Category)
                    .ToListAsync());
            }
        }

        public async Task<IEnumerable<ProductEntity>> GetByCategoryId(int categoryId, int pageNumber, int pageSize)
        {
            using (var db = GetContext())
            {
                return _mapper.Map<List<ProductEntity>>(await db.Set<Product>()
                    .Where(p => p.CategoryId == categoryId)
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .Include(p => p.Category)
                    .ToListAsync());
            }
        }
    }
}
