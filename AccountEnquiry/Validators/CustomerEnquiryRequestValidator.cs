using AccountEnquiry.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountEnquiry.Validators
{
    public class CustomerEnquiryRequestValidator : AbstractValidator<CustomerEnquiryRequest>
    {
        public CustomerEnquiryRequestValidator()
        {
            RuleFor(req => req.customerNumber)
                .NotNull()
                .NotEmpty()
                .MaximumLength(40);
        }
    }
}
