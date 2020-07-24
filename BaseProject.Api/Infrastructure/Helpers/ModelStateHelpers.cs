using BaseProject.Api.Infrastructure.Languages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace BaseProject.Api.Infrastructure.Helpers
{
    public static class ModelStateHelpers
    {
        public static string GetFirstError(this ModelStateDictionary modelState)
        {
            return modelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage ?? Messages.InvalidModel;
        }
    }
}
