using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RevenueAndExpense.BLL.Validator
{
    public class CustomChargeRequireAttribute: ValidationAttribute
    {
        private readonly string _compireKey;
        public CustomChargeRequireAttribute(string compireKey) // Pass the compare property name
        {
            this._compireKey = compireKey; // assing it to the private field.
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = Convert.ToDecimal(value); // Current Property value.

            var property = validationContext.ObjectType.GetProperty(_compireKey); // Get Compare Property
            if (property == null)
                throw new ArgumentException("Property with this name is not found"); // Raise an exception if the compare property is not found

            var propKey = (string)property.GetValue(validationContext.ObjectInstance); // Get compare property value.

            var currentKey = validationContext.MemberName; // get Current property name.

            var chargeProp = validationContext.ObjectType.GetProperty("ChargeName");
            var chargeName = (string)chargeProp.GetValue(validationContext.ObjectInstance);

            if (propKey == "Electricity" && currentValue <=0)
            {
                if (currentKey == "PreviousReading")
                    return new ValidationResult(ErrorMessage = "বিদ্যুৎতের পূর্বের রিডিং আবশ্যক");
                else if (currentKey == "CurrentReading")
                    return new ValidationResult(ErrorMessage = "বিদ্যুৎতের বর্তমান রিডিং আবশ্যক");
                else if (currentKey == "ConsumUnit")
                    return new ValidationResult(ErrorMessage = "বিদ্যুৎতের ব্যবহৃত ইউনিট আবশ্যক");
                else if (currentKey == "UnitRate")
                    return new ValidationResult(ErrorMessage = "বিদ্যুৎতের ইউনিট রেট আবশ্যক");
                else if (currentKey == "Amount")
                    return new ValidationResult(ErrorMessage = "বিদ্যুৎতের টাকার পরিমাণ আবশ্যক");
            }
            else if (propKey == "Fan" && (currentKey == "ConsumUnit" || currentKey == "UnitRate" || currentKey == "Amount") && currentValue <= 0)
            {
                if(currentKey == "ConsumUnit")
                    return new ValidationResult(ErrorMessage="ফ্যানের ব্যবহৃত ইউনিট আবশ্যক");
                else if(currentKey == "UnitRate")
                    return new ValidationResult(ErrorMessage = "ফ্যানের ইউনিট রেট আবশ্যক");
                else if (currentKey == "Amount")
                    return new ValidationResult(ErrorMessage = "ফ্যানের টাকার পরিমাণ আবশ্যক");
            }
            else if(propKey == "Others" && currentKey == "Amount" && currentValue <= 0)
            {
                return new ValidationResult(ErrorMessage = chargeName+" টাকার পরিমাণ আবশ্যক");
            }

            return ValidationResult.Success;
        }
    }
}