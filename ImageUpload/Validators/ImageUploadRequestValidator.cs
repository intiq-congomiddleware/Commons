using ImageUpload.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Validators
{
    public class ImageUploadRequestValidator : AbstractValidator<ImageUploadRequest>
    {
        public ImageUploadRequestValidator()
        {
            RuleFor(req => req.customerNo)
           .NotNull()
           .NotEmpty();

            RuleFor(req => req.userName)
            .NotNull()
            .NotEmpty();

            RuleFor(req => req.ImageData)
            .NotNull()
            .NotEmpty();

            RuleFor(req => req.ImageType)
            .NotNull()
            .NotEmpty()
            .SetValidator(new StatusEnumValidator());

            RuleFor(req => req.requestId)
           .NotNull()
           .NotEmpty();
        }
    }
}