using Microsoft.AspNetCore.Mvc;

namespace Sonar.UserProfile.Web.Tools;

public class ModelStateValidator
{
    public static IActionResult ValidateModelState(ActionContext context)
    {
        var entry = context.ModelState
            .Select(x => x.Value)
            .First(v => v.Errors.Count > 0);
        var errorSerialized = entry.Errors.First().ErrorMessage;

        return new BadRequestObjectResult(errorSerialized);
    }
}