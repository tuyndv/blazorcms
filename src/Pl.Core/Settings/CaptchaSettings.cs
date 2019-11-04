namespace Pl.Core.Settings
{
    public class CaptchaSettings
    {
        /// <summary>
        /// ReCaptchaPublicKey
        /// </summary>
        public string SiteKey { get; set; }

        /// <summary>
        /// ReCaptchaPrivateKey
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Giao diện captcha dark ,light
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Kiểu hiển thị của captcha có 2 dạng là audio , image
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Kích thước của dữ liệu có 2 dạng là compact và normal
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Bận captcha trên toàn hệ thống
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Hiển thị captcha trên trang login
        /// </summary>
        public bool ShowOnLoginPage { get; set; }

        /// <summary>
        /// Hiển thị captcha trên trang đăng ký user
        /// </summary>
        public bool ShowOnRegistrationPage { get; set; }

        /// <summary>
        /// Hiển thị captcha trên trang lấy lại mật khẩu
        /// </summary>
        public bool ShowOnForgotPasswordPage { get; set; }
    }
}