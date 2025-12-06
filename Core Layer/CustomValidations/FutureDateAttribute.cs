using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.CustomValidations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly expirationDate)
            {
                if (expirationDate < DateOnly.FromDateTime(DateTime.Today))
                {
                    return new ValidationResult("Expiration date must be today or a future date");
                }
            }

            return ValidationResult.Success;
        }
    }
}
