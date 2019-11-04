using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Pl.WebFramework
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Hàm sắp lại các lỗi thành một list
        /// </summary>
        /// <param name="modelStateDictionary">Mảng model state error</param>
        /// <returns>List string</returns>
        public static List<string> SerializeErrors(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Where(entry => entry.Value.Errors.Any()).SelectMany(q => SerializeModelState(q.Value.Errors)).ToList();
        }

        private static List<string> SerializeModelState(ModelErrorCollection modelState)
        {
            List<string> errors = new List<string>();
            for (int i = 0; i < modelState.Count; i++)
            {
                ModelError modelError = modelState[i];
                string errorText = ValidationHelpers.GetModelErrorMessageOrDefault(modelError);

                if (!string.IsNullOrEmpty(errorText))
                {
                    errors.Add(errorText);
                }
            }
            return errors;
        }
    }
}