using CatalogService.DAL.SQLiteDb.Base.Entities;
using CatalogService.DAL.SQLiteDb.CategoryRepository.Entities;

namespace CatalogService.DAL.SQLiteDb.ProductRepository.Entities
{
    public class Product : IdDbEntity
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public int CategoryId { get; set; }
        
        public Category? Category { get; set; }

        public double? Price { get; set; }

        public int? Amount { get; set; }
    }
}
