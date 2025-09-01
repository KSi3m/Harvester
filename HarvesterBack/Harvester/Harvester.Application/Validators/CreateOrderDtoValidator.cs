using FluentValidation;
using Harvester.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Validators
{
    public class CreateOrderDtoValidator: AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator() {
            RuleFor(x => x.FieldId)
               .NotEmpty()
                   .WithMessage("FieldId must not be empty")
                .GreaterThan(0)
                    .WithMessage("FieldId must greater than 0");

            RuleFor(x => x.CombineId)
               .NotEmpty()
                   .WithMessage("CombineId must not be empty")
                .GreaterThan(0)
                    .WithMessage("CombineId must greater than 0");

            RuleFor(x => x.OrderDate)
                .NotEmpty()
                   .WithMessage("OrderDate must not be empty")
                .GreaterThanOrEqualTo(DateTime.Now.Date)
                   .WithMessage("OrderDate cannot be in the past");
        }
    }
}
