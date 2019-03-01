using AccountOpening.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountOpening.Validators
{
    public class AccountOpeningRequestValidator : AbstractValidator<AccountOpeningRequest>
    {
        public AccountOpeningRequestValidator()
        {
        }
    }
}
