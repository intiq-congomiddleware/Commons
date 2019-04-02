using AccountEnquiry.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceEnquiry.Validators
{
    public class AccountEnquiryRequestValidator : AbstractValidator<AccountEnquiryRequest>
    {
        public AccountEnquiryRequestValidator()
        {
            RuleFor(req => req.accountNumber)
                .NotNull()
                .NotEmpty()
                .MaximumLength(15);
        }
    }
}
