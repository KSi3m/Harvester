using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Validators
{
    public class GetAreaValidator: AbstractValidator<string>
    {
        public GetAreaValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("NameIdentifier is required")
                .Matches(@"^\d{6}_\d\.\d{4}\.\d+(\/\d+)?$").WithMessage("NameIdentifier format is invalid. Please supply it in WWPPGG_R.XXXX.NDZ format");
        }
    }
}
