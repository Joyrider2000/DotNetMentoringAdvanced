using CartingService.DAL.Entities;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CartingService.DAL.Validators
{
    internal class CartItemValidator : ValidationAttribute
    {
        public override bool IsValid(object? cartItems)
        {
            if (cartItems is not IEnumerable list) return true;

            var isValid = true;

            foreach (var item in list)
            {
                if (item is CartItem cartItem)
                {
                    isValid &= cartItem.isValid();
                }
            }
            return isValid;
        }
    }
}
