using CatalogService.DAL.SQLiteDb.Base.Repositories;
using CatalogService.DAL.SQLiteDb.ProductRepository.Entities;
using CatalogService.DAL.SQLiteDb.ProductRepository.Mappers;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.Domain.Entities;

namespace CatalogService.DAL.SQLiteDb.ProductRepository
{
    public class ProductRepository : BaseRepository<Product, ProductEntity>
    {
        public ProductRepository(string connectionString) : base(connectionString, ProductMapperBuilder.Build()) { }

        protected override ProductEntity GetById(DbContext db, int id)
        {
            return _mapper.Map<ProductEntity>(db.Set<Product>().Include(p => p.Category).FirstOrDefault(p => p.Id == id));
        }

        protected override IEnumerable<Product> GetAll(DbContext db)
        {
            return db.Set<Product>().Include(p => p.Category).ToList();
        }
    }
}
