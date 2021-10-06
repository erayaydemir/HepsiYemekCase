using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HepsiYemek.WebApi.Filters
{
    public class HexObjectIdAttribute :ValidationAttribute
    {
        private string _errorMessage;

        public HexObjectIdAttribute(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var objectId = value.ToString();
            if (Regex.IsMatch(objectId, @"^[0-9a-fA-F]{24}$"))
                return ValidationResult.Success;
            else
            {
                this.ErrorMessage = _errorMessage;
                return new ValidationResult(_errorMessage);
            }
                
        }
    }
}
