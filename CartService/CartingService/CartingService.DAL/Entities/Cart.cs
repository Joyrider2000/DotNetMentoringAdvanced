using CartingService.DAL.Validators;
using System.ComponentModel.DataAnnotations;

namespace CartingService.DAL.Entities
{
    public class Cart : ValidatableEntity
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        [CartItemValidator]
        public List<CartItem>? Items { get; set; }
    }
}