using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CatalogService.BLL.Domain.Entities
{
    public class ProductEntity : IdEntity
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Url]
        public string? Image { get; set; }

        [Required]
        public CategoryEntity? Category { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? Amount { get; set; }
    }
}
