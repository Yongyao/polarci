using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.DataAnnotations;

namespace GeoSearch
{
    public class CustomizeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int)
            {
                int value_int = (int)value;
                if (value_int > 10000 || value_int < 100)
                {
                    return new ValidationResult("Please input a positive integer between 100 and 10000");
                }
                return ValidationResult.Success;
            }
            return ValidationResult.Success;
        }
    }
}
