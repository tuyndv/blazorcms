using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Pl.WebFramework.FilterAttributes
{
    public class ExceptionAttribute : TypeFilterAttribute
    {
        public ExceptionAttribute() : base(typeof(ExceptionFilter))
        {
        }

        /// <summary>
        /// Lớp này xử lý ghi log toàn bộ các exception trong hệ thống
        /// </summary>
        private class ExceptionFilter : IExceptionFilter
        {
            private readonly ILogger<ExceptionFilter> _logger;

            public ExceptionFilter(ILogger<ExceptionFilter> logger)
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
                context.ExceptionHandled = false;
            }
        }
    }
}