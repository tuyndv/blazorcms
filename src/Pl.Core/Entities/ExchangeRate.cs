using System;

namespace Pl.Core.Entities
{
    /// <summary>
    /// Tỉ giá tiền tệ
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// Khởi tạo tỉ giá
        /// </summary>
        public ExchangeRate()
        {
            CurrencyCode = string.Empty;
            Rate = 1.0m;
        }

        /// <summary>
        /// Mã tiền tện chuẩn 3 ký tự vd USD, VNĐ
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Tỉ giá
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Ngày update
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}