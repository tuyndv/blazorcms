namespace Pl.Core.Entities
{
    public enum ActivityTypeEnum
    {
        /// <summary>
        /// Hệ thống
        /// </summary>
        System,

        /// <summary>
        /// Khởi động lại
        /// </summary>
        Restart,

        /// <summary>
        /// Xóa cache
        /// </summary>
        ClearCache,

        /// <summary>
        /// Thêm mới
        /// </summary>
        Insert,

        /// <summary>
        /// Sửa
        /// </summary>
        Update,

        /// <summary>
        /// Xóa
        /// </summary>
        Delete,

        /// <summary>
        /// Đăng nhập
        /// </summary>
        Login,

        /// <summary>
        /// Đăng xuất
        /// </summary>
        Logout,

        /// <summary>
        /// Xem danh sách
        /// </summary>
        ViewList,

        /// <summary>
        /// Xem chi tiết
        /// </summary>
        ViewDetails,

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        ChangePassword,

        /// <summary>
        /// Đăng ký
        /// </summary>
        Register,

        /// <summary>
        /// Kích hoạt người dùng
        /// </summary>
        UserActive,

        /// <summary>
        /// Yêu cầu đổi mật khẩu
        /// </summary>
        PasswordRecovery,

        /// <summary>
        /// Đổi thông tin cá nhân
        /// </summary>
        UserProfile,

        /// <summary>
        /// Nhập dữ liệu
        /// </summary>
        Import,

        /// <summary>
        /// Xuất dữ liệu
        /// </summary>
        Export,

        /// <summary>
        /// Đổi thông tin cá nhân
        /// </summary>
        SetDefaultLanguage,

        /// <summary>
        /// Đổi thông tin cá nhân
        /// </summary>
        SetDefaultCurrency
    }

    public class ActivityLog : BaseEntity
    {
        /// <summary>
        /// Id người thực hiện
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Tin nhắn thông báo.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Trạng thái thành công
        /// </summary>
        public bool Complete { get; set; }

        /// <summary>
        /// Loại hành động
        /// </summary>
        public ActivityTypeEnum Type { get; set; }

        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public ObjectTypeEnum ObjectType { get; set; }

        /// <summary>
        /// Đối tượng
        /// </summary>
        public string ObjectId { get; set; } = null;

        /// <summary>
        /// Object sau khi thực hiện
        /// Có thể null
        /// </summary>
        public string ObjectJson { get; set; }

        /// <summary>
        /// Địa chỉ ip máy client gây lỗi
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Happening action url
        /// </summary>
        public string PageUrl { get; set; }
    }
}