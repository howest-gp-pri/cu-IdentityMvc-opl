using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddCustomModelErrors(this ModelStateDictionary modelState,IEnumerable<string> errors)
        {
            if (errors.Count() > 1)
            {
                foreach (var error in errors)
                {
                    modelState.AddModelError("", error);
                }
            }
            else
            {
                modelState.AddModelError("",errors.First());
            }
        }
    }
}
