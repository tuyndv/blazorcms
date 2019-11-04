using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Net;

namespace Pl.WebFramework.FilterAttributes
{
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        private readonly string[] submitButtonNames;
        private readonly bool validateNameOnly;

        public FormValueRequiredAttribute(params string[] _submitButtonNames) : this(true, _submitButtonNames)
        {
        }

        public FormValueRequiredAttribute(bool _validateNameOnly, params string[] _submitButtonNames)
        {
            submitButtonNames = _submitButtonNames;
            validateNameOnly = _validateNameOnly;
        }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            if (routeContext.HttpContext.Request.Method != WebRequestMethods.Http.Post)
            {
                return false;
            }

            foreach (string buttonName in submitButtonNames)
            {
                if (validateNameOnly)
                {
                    if (routeContext.HttpContext.Request.Form.Keys.Any(x => x.Equals(buttonName, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        return true;
                    }
                }
                else
                {
                    string value = routeContext.HttpContext.Request.Form[buttonName];
                    if (!string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}