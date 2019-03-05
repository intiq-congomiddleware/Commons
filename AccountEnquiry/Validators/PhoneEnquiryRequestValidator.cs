using AccountEnquiry.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountEnquiry.Validators
{
    public class PhoneEnquiryRequestValidator : AbstractValidator<PhoneEnquiryRequest>
    {
        public PhoneEnquiryRequestValidator()
        {
        }
    }
}
