using FluentValidation;
using Harvester.Application.Dtos;

namespace Harvester.Application.Validators
{
    public class CreateFieldDtoValidator: AbstractValidator<CreateFieldDto>
    {
        public CreateFieldDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name must not be empty")
                .Matches(@"^\d{6}_\d\.\d{4}\.\d+(\/\d+)?$")
                    .WithMessage("Code format is invalid")
                .MaximumLength(100)
                    .WithMessage("Name must have less than 100 characters");

            RuleFor(x => x.AreaHectares)
                .NotEmpty()
                    .WithMessage("AreaHectares must not be empty")
                .GreaterThan(0.01m)
                     .WithMessage("AreaHectares must greater than 0.01");

            RuleFor(x => x.TerrainCoeff).NotEmpty()
                .WithMessage("Terrain Coeff must not be empty");

            RuleFor(x => x.ShapeCoeff).NotEmpty()
                .WithMessage("Shape Coeff must not be empty");

            RuleFor(x => x.CropType).NotEmpty()
                .WithMessage("Crop type must not be empty");
        }
    }
}
