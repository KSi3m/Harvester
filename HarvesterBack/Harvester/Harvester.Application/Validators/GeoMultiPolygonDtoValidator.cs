using FluentValidation;
using Harvester.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Validators
{
    public class GeoMultiPolygonDtoValidator : AbstractValidator<GeoMultiPolygonDto>
    {
        public GeoMultiPolygonDtoValidator() {

            RuleFor(x => x.Type)
                .Equal("MultiPolygon")
                .WithMessage("Type must be 'MultiPolygon'");

            RuleFor(x => x.Coordinates)
                .NotEmpty()
                    .WithMessage("Coordinates must not be empty")
                .Must(x => x.Length > 0)
                    .WithMessage("Coordinates must contain at least one polygon");
        }
    }
    
}
