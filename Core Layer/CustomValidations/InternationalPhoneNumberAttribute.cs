using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.CustomValidations
{
    public class InternationalPhoneNumberAttribute : ValidationAttribute
    {
        private const string DefaultRegion = "EG"; // Default region for parsing if no country code is present (+XX)

        public InternationalPhoneNumberAttribute()
            : base("The phone number is not a valid international or Egyptian number.")
        {
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return ValidationResult.Success; // Allow null or empty for optional fields
            }

            try
            {
                var util = PhoneNumberUtil.GetInstance();

                // 1. Try to parse the number
                // We use 'EG' (Egypt) as a default region to help parse domestic formats (like 010...)
                var number = util.Parse(phoneNumber, DefaultRegion);

                // 2. Check if the parsed number is a valid number *type* (e.g., mobile, fixed line, etc.)
                // AND ensure it's a number that can be dialled.
                if (util.IsValidNumber(number))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    // If parsing succeeded but the number is invalid (e.g., too many/few digits)
                    return new ValidationResult(ErrorMessage);
                }
            }
            catch (NumberParseException)
            {
                // If the number couldn't be parsed at all (e.g., junk input)
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
