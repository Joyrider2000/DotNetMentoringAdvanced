using CartingService.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CartingService.CartingService.DAL.Validators
{
    internal class ImageValidator : ValidationAttribute
    {
        public override bool IsValid(object? image)
        { 
            return image == null || ((Image)image).isValid();
        }
    }
}
