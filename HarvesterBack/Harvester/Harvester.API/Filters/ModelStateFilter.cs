using Harvester.Application.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Harvester.API.Middlewares
{
    public class ModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = string.Join("; ",
                      context.ModelState
                          .Where(ms => ms.Value.Errors.Any())
                          .SelectMany(ms => ms.Value.Errors.Select(e =>
                              $"{ms.Key}: {e.ErrorMessage}"))
                  );

                throw new ModelStateException(errors);
            }
        }
    }
}
