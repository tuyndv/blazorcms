using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Pl.WebFramework.FilterAttributes
{
    /// <summary>
    /// Lớp kế thừa và ghi lại phương thức chứng thực anti forgenry cho khu vực areas admin
    /// </summary>
    public class AntiForgeryAttribute : TypeFilterAttribute
    {
        public AntiForgeryAttribute(bool _ignore = false) : base(typeof(AntiForgeryFilter))
        {
            Arguments = new object[] { _ignore };
        }

        private class AntiForgeryFilter : ValidateAntiforgeryTokenAuthorizationFilter
        {
            private readonly bool ignore;

            public AntiForgeryFilter(bool _ignore, IAntiforgery antiforgery, ILoggerFactory loggerFactory) : base(antiforgery, loggerFactory)
            {
                ignore = _ignore;
            }

            protected override bool ShouldValidate(AuthorizationFilterContext context)
            {
                if (ignore)
                {
                    return false;
                }

                HttpRequest request = context.HttpContext.Request;
                if (request == null)
                {
                    return false;
                }

                if (request.Method.Equals(WebRequestMethods.Http.Get, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                return base.ShouldValidate(context);
            }
        }
    }
}