﻿using System.ComponentModel.DataAnnotations;

namespace CatalogService.BLL.Domain.Entities
{
    public class CategoryEntity : IdEntity
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Url]
        public string? Image { get; set; }

        public CategoryEntity? Parent { get; set; }
    }
}