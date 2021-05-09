using FluentValidation;
using GreenFluxAssignment.Api.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Api.Validators
{
    public class SaveConnectorResourceValidator : AbstractValidator<SaveConnectorResource>
    {
        public SaveConnectorResourceValidator()
        {
            RuleFor(a => a.MaxCurrent)
                .NotNull()
                .NotEmpty()
                .WithMessage("Current can not be null or empty");

            RuleFor(a => a.MaxCurrent)
                .GreaterThan(0)
                .WithMessage("Current must be greater than zero");
        }
    }
}
