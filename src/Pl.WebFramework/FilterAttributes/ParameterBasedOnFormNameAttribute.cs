using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Pl.WebFramework.FilterAttributes
{
    public class ParameterBasedOnFormNameAttribute : TypeFilterAttribute
    {
        public ParameterBasedOnFormNameAttribute(string _formKeyNames, string _actionParameterNames) : base(typeof(ParameterBasedOnFormNameFilter))
        {
            Arguments = new object[] { _formKeyNames, _actionParameterNames };
        }

        private class ParameterBasedOnFormNameFilter : IActionFilter
        {
            private readonly string formKeyNames;
            private readonly string actionParameterNames;

            public ParameterBasedOnFormNameFilter(string _formKeyNames, string _actionParameterNames)
            {
                formKeyNames = _formKeyNames;
                actionParameterNames = _actionParameterNames;
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                string[] listformKey = formKeyNames.Split(',');
                string[] listParameterName = actionParameterNames.Split(',');
                for (int i = 0; i < listformKey.Length; i++)
                {
                    context.ActionArguments[listParameterName[i]] = context.HttpContext.Request.Form.Keys.Any(key => key.Equals(listformKey[i]));
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
            }
        }
    }
}