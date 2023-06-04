using CatalogService.DAL.SQLiteDb.Base.Repositories;
using CatalogService.DAL.SQLiteDb.CategoryRepository.Entities;
using CatalogService.DAL.SQLiteDb.CategoryRepository.Mappers;
using Microsoft.EntityFrameworkCore;
using CatalogService.BLL.Domain.Entities;
using CatalogService.BLL.Domain.Exceptions;
using CatalogService.DAL.SQLiteDb.Base.Entities;
using Microsoft.Data.Sqlite;
using CatalogService.DAL.SQLiteDb.ProductRepository.Entities;
using Serilog;
using CatalogService.DAL.Configuration.Options;
using Microsoft.Extensions.Logging;
using CatalogService.BLL.Application.Repositories;

namespace CatalogService.DAL.SQLiteDb.CategoryRepository
{
    public class CategoryRepository : BaseRepository<Category, CategoryEntity>
    {
        public CategoryRepository(AppOptions appOptions, ILogger<CategoryRepository> logger) : base(appOptions, CategoryMapperBuilder.Build(), logger) { }

        protected override async Task<CategoryEntity> GetById(DbContext db, int id)
        {
            return _mapper.Map<CategoryEntity>(await db.Set<Category>().Include(p => p.Parent).FirstOrDefaultAsync(p => p.Id == id));
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                using (var db = GetContext())
                {
                    await db.Set<Category>().Where(c => c.ParentId == id).AsQueryable().ForEachAsync(async c =>
                    {
                        await Delete(c.Id);
                    });

                    db.Set<Product>().Where(p => p.CategoryId == id).ExecuteDelete();
                    db.Set<Category>().Where(c => c.Id == id).ExecuteDelete();
                    await db.SaveChangesAsync();

                    _logger.LogInformation($"Successfully deleted {typeof(Category)} #{id}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqliteException)
                {
                    var sqliteException = (SqliteException)ex.InnerException;
                    if (sqliteException.SqliteErrorCode == SQLITE_CONSTRAINT)
                    {
                        throw new ConstraintViolationException();
                    }
                }
                _logger.LogInformation($"Error deleting {typeof(Category)} #{id}: {ex}");
                return false;
            }
        }
    }
}
