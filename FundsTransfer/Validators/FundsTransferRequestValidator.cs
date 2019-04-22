using FluentValidation;
using FundsTransfer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundsTransfer.Validators
{
    public class FundsTransferRequestValidator : AbstractValidator<FundsTransferRequest>
    {
        public FundsTransferRequestValidator()
        {
            RuleFor(req => req.cract)
                   .MaximumLength(20);
                   //.When(x => !string.IsNullOrEmpty(x.cract));

            RuleFor(req => req.cract1)
                   .NotNull()
                   .NotEmpty()
                   .MaximumLength(20)
                   .When(x => x.with_charges);

            RuleFor(req => req.cract2)
                   .NotNull()
                   .NotEmpty()
                   .MaximumLength(20)
                   .When(x => x.with_charges);

            RuleFor(req => req.dract)
                  .MaximumLength(20);
                  //.When(x => !string.IsNullOrEmpty(x.cract));

            RuleFor(req => req.trnamt)
                    .NotNull()
                    .NotEmpty()
                    .GreaterThan(0);

            RuleFor(req => req.txnnarra)
                    .NotNull()
                    .NotEmpty()
                    .MaximumLength(100);

            RuleFor(req => req.product)
                    .NotNull()
                    .NotEmpty()
                    .MaximumLength(100);

            RuleFor(req => req.l_acs_ccy)
                  .NotNull()
                  .NotEmpty()
                  .MaximumLength(100)
                  .SetValidator(new CurrencyEnumValidator());

            RuleFor(req => req.instr_code)
                  .NotNull()
                  .NotEmpty()
                  .MaximumLength(100);

            RuleFor(req => req.branch_code)
                  .NotNull()
                  .NotEmpty()
                  .MaximumLength(100);

            RuleFor(req => req.user_name)
                  .NotNull()
                  .NotEmpty()
                  .MaximumLength(100);

            RuleFor(req => req.prate)
                  .NotNull()
                  .NotEmpty()
                  .GreaterThan(0)
                  .When(x => x.with_charges);
        }
    }
}