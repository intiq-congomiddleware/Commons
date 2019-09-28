using CurrencyRates.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebitFreeze.Validators
{
    public class CurrencyRequestValidator : AbstractValidator<CurrencyRequest>
    {
        public CurrencyRequestValidator()
        {

            RuleFor(req => req.userName)
                    .NotNull()
                    .NotEmpty();
        }
    }
}