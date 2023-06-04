using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL.Entities
{
    public class Image : ValidatableEntity
    {
        [Url]
        public string? URL { get; set; }

        public string? AltText { get; set; }  
    }
}
