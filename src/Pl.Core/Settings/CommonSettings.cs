namespace Pl.Core.Settings
{
    public class CommonSettings
    {
        /// <summary>
        /// Trạng thái cài đặt của hệ thống
        /// </summary>
        public bool IsSystemInstalled { get; set; }

        /// <summary>
        /// Link to  document website
        /// </summary>
        public string DocumentWebsite { get; set; }

        /// <summary>
        /// Phiên bản của hệ thống
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Key mã hóa của toàn bộ hệ thống
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Số dòng bản ghi mặc định của các lưới trong trang quản trị
        /// </summary>
        public int CmsPageSize { get; set; }

        /// <summary>
        /// Chuỗi ký tự thể hiện liên hệ cấp cha đến con
        /// </summary>
        public string ParentToChildString { get; set; }

        /// <summary>
        /// Danh sách email quản trị nhận được khi hệ thống chết
        /// </summary>
        public string AdminEmailsSystemNotify { get; set; }

        /// <summary>
        /// Chữ ký của hệ thống
        /// </summary>
        public string EmailSignature { get; set; }

    }
}