using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Pl.WebFramework.Models;
using System.Net;

namespace Pl.WebFramework.FilterAttributes
{
    public class ApiExceptionAttribute : TypeFilterAttribute
    {
        public ApiExceptionAttribute() : base(typeof(ApiExceptionFilter))
        {
        }

        /// <summary>
        /// Lớp này xử lý ghi log toàn bộ các exception trong hệ thống
        /// </summary>
        private class ApiExceptionFilter : IExceptionFilter
        {
            private readonly ILogger<ApiExceptionFilter> _logger;

            public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
            {
                _logger = logger;
            }

            /// <summary>
            /// Xử lý khi yêu cầu phía client đến máy chủ và máy chủ xử lý bị lỗi
            /// </summary>
            /// <param name="context">Lỗi phát sinh</param>
            public void OnException(ExceptionContext context)
            {
                try
                {
                    _logger.LogError(context.Exception, context.Exception.Message);
                }
                catch
                {
                    //Không xử lý khi gặp lỗi ở đây
                }
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(new ApiResponseData()
                {
                    Code = -1,
                    Message = "Gặp lỗi trong quá trình xử lý.",
                    Data = null
                });
                context.ExceptionHandled = true;
            }
        }
    }
}