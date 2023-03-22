using CatalogService.DAL.SQLiteDb.Base.Repositories;
using CatalogService.DAL.SQLiteDb.CategoryRepository.Entities;
using CatalogService.DAL.SQLiteDb.CategoryRepository.Mappers;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.Domain.Entities;

namespace CatalogService.DAL.SQLiteDb.CategoryRepository
{
    public class CategoryRepository : BaseRepository<Category, CategoryEntity>
    {
        public CategoryRepository(string connectionString) : base(connectionString, CategoryMapperBuilder.Build()) { }

        protected override CategoryEntity GetById(DbContext db, int id)
        {
            return _mapper.Map<CategoryEntity>(db.Set<Category>().Include(p => p.Parent).FirstOrDefault(p => p.Id == id));
        }
    }
}
