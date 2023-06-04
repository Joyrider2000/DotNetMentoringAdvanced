using System.ComponentModel.DataAnnotations;

namespace CartingService.DAL.Entities
{
    public abstract class ValidatableEntity
    {
        public bool isValid()
        {
            ValidationContext context = new ValidationContext(this);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(this, context, validationResults, true);
        }
    }
}