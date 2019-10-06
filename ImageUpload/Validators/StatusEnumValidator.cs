using FluentValidation.Validators;
using ImageUpload.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Validators
{
    public class StatusEnumValidator : PropertyValidator
    {

        public StatusEnumValidator()
            : base("Invalid {PropertyName}, {enumValue}.")
        {
        }
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var enumString = context.PropertyValue as string;


            //Confirm valid date then check if date is greater than or equal to now.
            if (string.IsNullOrEmpty(enumString))
            {
                context.MessageFormatter.AppendArgument("enumValue", enumString);
                return false;
            }

            if (!Enum.IsDefined(typeof(ImageTypes), enumString.ToUpper()))
            {
                context.MessageFormatter.AppendArgument("enumValue", enumString);
                return false;
            }
            return true;
        }
    }
}
