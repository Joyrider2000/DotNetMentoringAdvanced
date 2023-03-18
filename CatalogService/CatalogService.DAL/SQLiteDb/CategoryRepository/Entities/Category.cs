using CatalogService.DAL.SQLiteDb.Base.Entities;

namespace CatalogService.DAL.SQLiteDb.CategoryRepository.Entities
{
    public class Category : IdDbEntity
    {
        public string? Name { get; set; }

        public string? Image { get; set; }

        public int? ParentId { get; set; }

        public Category? Parent { get; set; }
    }
}