namespace Pl.Core.Settings
{
    /// <summary>
    /// tất cả các url cầu hình ở đầu đều bắt đầu bằng dấu /
    /// </summary>
    public class UrlSettings
    {
        #region System

        /// <summary>
        /// Url gốc trang cms
        /// </summary>
        public string CmsDomain { get; set; }

        /// <summary>
        /// Url gốc trang hiển thị
        /// </summary>
        public string WebDomain { get; set; }

        /// <summary>
        /// Url gốc trang mobile
        /// </summary>
        public string MobileDomain { get; set; }

        /// <summary>
        /// Url Gốc trang Api
        /// </summary>
        public string ApiDomain { get; set; }

        /// <summary>
        /// Url trang nỗi 404
        /// </summary>
        public string FileNotFoundPage { get; set; }

        /// <summary>
        /// Trang lỗi
        /// </summary>
        public string ErrorPage { get; set; }

        /// <summary>
        /// Url trang hỗ trợ quản trị
        /// </summary>
        public string SupportSite { get; set; }

        #endregion System

        #region Membership

        /// <summary>
        /// Url trang đăng nhập
        /// </summary>
        public string LoginPage { get; set; }

        /// <summary>
        /// Url trang đăng xuất
        /// </summary>
        public string LogoutPage { get; set; }

        /// <summary>
        /// Url trang thay đổi mật khẩu
        /// </summary>
        public string PasswordChangerPage { get; set; }

        /// <summary>
        /// Url trang phục hồi mật khẩu
        /// </summary>
        public string ForgotPasswordPage { get; set; }

        /// <summary>
        /// Url trang đăng ký thành viên
        /// </summary>
        public string RegisterPage { get; set; }

        /// <summary>
        /// Url trang kích hoạt thành viên
        /// </summary>
        public string UserActivePage { get; set; }

        /// <summary>
        /// Url trang cấm truy cập
        /// </summary>
        public string AccessDeniedPage { get; set; }

        #endregion Membership
    }
}