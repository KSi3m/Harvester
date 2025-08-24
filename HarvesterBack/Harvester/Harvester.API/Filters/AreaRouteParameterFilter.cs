using FluentValidation;
using FluentValidation.Results;
using Harvester.Application.Exceptions;
using Harvester.Application.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Harvester.API.Filters
{
    public class AreaRouteParameterFilter(GetAreaValidator codeValidator) : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("nameIdentifier", out var idObj) && idObj is string id)
            {
                ValidationResult result = codeValidator.Validate(id);
                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

                    throw new ModelStateException(string.Join("; ", errors));
                }
            }
        }
    }
}
