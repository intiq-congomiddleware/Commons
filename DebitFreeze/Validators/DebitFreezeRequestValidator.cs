using DebitFreeze.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebitFreeze.Validators
{
    public class DebitFreezeRequestValidator : AbstractValidator<DebitFreezeRequest>
    {
        public DebitFreezeRequestValidator()
        {

            RuleFor(req => req.accountNumber)
                    .NotNull()
                    .NotEmpty();
        }
    }
}