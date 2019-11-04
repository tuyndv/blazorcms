using Microsoft.Extensions.Logging;

namespace Pl.Core.Entities
{
    public class SystemLog : BaseEntity
    {

        /// <summary>
        /// Mức độ lỗi
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// Mô tả đầy đủ
        /// </summary>
        public string FullMessage { get; set; } = "";

        /// <summary>
        /// UserAgent of browser
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Địa chỉ ip máy client gây lỗi
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Url trang gây lỗi
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// Url liên quan gây lỗi
        /// </summary>
        public string ReferrerUrl { get; set; }
    }
}