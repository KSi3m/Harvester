using FluentValidation;
using Harvester.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Validators
{
    public class CreateCombineDtoValidator : AbstractValidator<CreateCombineDto>
    {
        public CreateCombineDtoValidator()
        {
            RuleFor(x => x.Model)
              .NotEmpty()
                .WithMessage("Model name must not be empty")
              .MaximumLength(100)
                .WithMessage("Model name must have less than 100 characters");

            RuleFor(x => x.BaseHaPerHour)
                .NotEmpty()
                    .WithMessage("BaseHaPerHour must not be empty")
                .GreaterThan(0.01m)
                     .WithMessage("BaseHaPerHour must be greater than 0.01");

            RuleFor(x => x.HeaderLength)
                .NotEmpty()
                    .WithMessage("HeaderLength must not be empty")
                .GreaterThan(1m)
                     .WithMessage("HeaderLength must be greater than 1 meter");

            RuleFor(x => x.PricePerHectare)
                .NotEmpty()
                    .WithMessage("PricePerHectare must not be empty")
                .GreaterThan(0)
                     .WithMessage("PricePerHectare must be greater than 0 pln per ha")
                .LessThan(5000)
                     .WithMessage("PricePerHectare must be lees than 5000 pln per ha");

            RuleFor(x => x.HasStrawChopper)
                .NotNull()
                    .WithMessage("Availability of straw chopper must be set");

            RuleFor(x => x.IsAvailable)
                .NotNull()
                    .WithMessage("Combine availability must be set");

            RuleFor(x => x.AvailableWorkHours)
                .NotEmpty()
                    .WithMessage("AvailableWorkHours must not be empty")
                .GreaterThan(1)
                     .WithMessage("AvailableWorkHours must be greater than 1")
                .LessThanOrEqualTo(24)
                     .WithMessage("AvailableWorkHours must be less than or equal 24");


            RuleFor(x => x.BaseEfficency)
               .NotEmpty()
                   .WithMessage("BaseEfficency must not be empty")
               .GreaterThan(0.01m)
                    .WithMessage("BaseEfficency must be greater than 0.01")
                .LessThanOrEqualTo(1m)
                    .WithMessage("BaseEfficency must be less than  or equal 1");
        }
    }
}
