using FluentValidation;
using GreenFluxAssignment.Api.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFluxAssignment.Api.Validators
{
    public class SaveChargeStationResourceValidator : AbstractValidator<SaveChargeStationResource>
    {
        public SaveChargeStationResourceValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("The name can not be null or empty");

            RuleFor(x => x.Connectors.Count)
                .GreaterThan(0)
                .WithMessage("Charge station has to have at least one connector");
        }
    }
}
