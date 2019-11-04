namespace Pl.Core.Entities
{
    public class EmailAccount : BaseEntity
    {

        /// <summary>
        /// Địa chỉ email của tài khoản
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// domain của tài khoản
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Cổng gửi email của tài khoản
        /// </summary>
        public int Port { get; set; } = 21;

        /// <summary>
        /// Tài khoản
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Tài khoản gửi email dạng ssl hay không
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Sử chứng thực mặc định hay không
        /// </summary>
        public bool UseDefaultCredentials { get; set; } = false;

        /// <summary>
        /// Thứ tự xuất hiện trong các danh sách
        /// </summary>
        public int DisplayOrder { get; set; } = 1;

        /// <summary>
        /// Được sử dụng làm mặc định để gửi có thể set nhiều
        /// </summary>
        public bool UseDefault { get; set; } = false;

        /// <summary>
        /// Hạn mức gửi
        /// </summary>
        public int DaySendLimit { get; set; } = 100;

        /// <summary>
        /// Ngày gửi cuối cùng, trường này lưu ngày gửi. để tính hạn mức gửi theo ngày
        /// </summary>
        public string DaySendLimited { get; set; } = "";

        /// <summary>
        /// Lưu số lần đã gửi theo daySen
        /// </summary>
        public int SendCountByDay { get; set; } = 0;
    }
}