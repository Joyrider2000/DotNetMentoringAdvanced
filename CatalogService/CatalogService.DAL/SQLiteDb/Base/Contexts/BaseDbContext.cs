using CatalogService.DAL.SQLiteDb.CategoryRepository.Entities;
using CatalogService.DAL.SQLiteDb.ProductRepository.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.DAL.SQLiteDb.Base.Contexts
{
    public class BaseDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        
        public BaseDbContext(string connectionString)
        {
            _connectionString = connectionString;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}