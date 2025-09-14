using FluentValidation;
using Harvester.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Validators
{
    public class GeoPointDtoValidator : AbstractValidator<GeoPointDto>
    {

        public GeoPointDtoValidator() {
            RuleFor(x => x.Type)
                .Equal("Point")
                .WithMessage("Type must be 'Point'");

            RuleFor(x => x.Coordinates)
                .NotEmpty()
                    .WithMessage("Coordinates must not be empty")
                .Must(x => x.Length == 2)
                    .WithMessage("Coordinates must be in format [number,number]");
        }
    }
}
