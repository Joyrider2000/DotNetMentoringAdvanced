using CartingService.CartingService.DAL.Validators;
using System.ComponentModel.DataAnnotations;

namespace CartingService.DAL.Entities
{
    public class CartItem : ValidatableEntity
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [ImageValidator]
        public Image? Image { get; set; }

        [Required]
        [Range(double.Epsilon, (double)decimal.MaxValue)]
        public decimal? Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? Quantity { get; set; }

    }
}
