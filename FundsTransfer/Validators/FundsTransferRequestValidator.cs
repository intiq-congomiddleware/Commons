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
        }
    }
}