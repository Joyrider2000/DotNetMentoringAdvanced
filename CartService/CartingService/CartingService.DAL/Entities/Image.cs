using System.ComponentModel.DataAnnotations;

namespace CartingService.DAL.Entities
{
    public class Image : ValidatableEntity
    {
        [Url]
        public string? URL { get; set; }

        public string? AltText { get; set; }  
    }
}
