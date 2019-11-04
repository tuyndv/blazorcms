namespace Pl.Core.Entities
{
    public class Currency : BaseEntity
    {

        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã tiền tệ dạng USD, VND ...
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Tỉ giá
        /// </summary>
        public decimal Rate { get; set; } = 1;

        /// <summary>
        /// Vùng miền hiển thị dang en-US, vi-VN ...
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Tùy chỉnh hiển thị giá theo định dạng
        /// </summary>
        public string CustomFormatting { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; } = 1;

        /// <summary>
        /// Trạng thái đăng
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Là tỉ giá chính trong hệ thống
        /// </summary>
        public bool IsPrimaryExchange { get; set; }

        /// <summary>
        /// Là tiền tệ chính trong hệ thống
        /// </summary>
        public bool IsPrimarySystem { get; set; }
    }
}