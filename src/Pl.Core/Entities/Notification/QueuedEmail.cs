using System;

namespace Pl.Core.Entities
{
    /// <summary>
    /// Email thông báo
    /// </summary>
    public class QueuedEmail : BaseEntity
    {
        /// <summary>
        /// Mức độ quan trọng
        /// Thang từ 1 đến 10
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Địa chỉ email người nhận
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Tên người nhận
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// danh sách email cc phân cách nhau bằng dấu ,
        /// </summary>
        public string Cc { get; set; }

        /// <summary>
        /// Danh sách email bcc phân cách nhau bằng dấu ,
        /// </summary>
        public string Bcc { get; set; }

        /// <summary>
        /// Tiêu đề của email
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nội dung của email
        /// </summary>
        public string EmailBody { get; set; }

        /// <summary>
        /// Số lần cố gắng gửi
        /// </summary>
        public int TrySend { get; set; }

        /// <summary>
        /// Thời gian gửi thành công
        /// </summary>
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// Trạng thái đã gửi hay chưa của email
        /// </summary>
        public bool IsSend => SendTime.HasValue;

        /// <summary>
        /// id email account đã gửi email này
        /// </summary>
        public string EmailAccountId { get; set; } = null;
    }
}