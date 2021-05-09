using FluentValidation;
using GreenFluxAssignment.Api.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Api.Validators
{
    public class SaveGroupResourceValidator : AbstractValidator<SaveGroupResource>
    {
        public SaveGroupResourceValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("The name can not be null");

            RuleFor(x => x.Capacity)
                .NotNull()
                .NotEmpty()
                .WithMessage("The capacity can not be null or empty");

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .WithMessage("The capacity must be greater than zero.");
        }
    }
}
