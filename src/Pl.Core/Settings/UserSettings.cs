using System.Globalization;

namespace Pl.Core.Settings
{
    public class UserSettings
    {
        /// <summary>
        /// Danh sách các kích thước ảnh avatar khi tạo ảnh thumb, ví dụ 100,120*120
        /// </summary>
        public string AvatarResizeList { get; set; }

        /// <summary>
        /// Đường dẫn ảnh avatar trong hệ thống.
        /// </summary>
        public string DomainAvatarPath { get; set; }

        /// <summary>
        /// Đường dẫn upload ảnh avatar trong hệ thống.
        /// </summary>
        public string UploadAvatarPath { get; set; }

        /// <summary>
        /// Kích thước tối đa của ảnh upload lên
        /// </summary>
        public int AvatarImageMaxSize { get; set; }

        /// <summary>
        /// Danh sách đuôi file ảnh hợp lệ trong avatar phân cách nhau bằng 1 dấu ,
        /// </summary>
        public string AvatarImageAllowedExtensions { get; set; }

        /// <summary>
        /// Thời gian một người dùng được coi là mới tính bằng giây
        /// </summary>
        public int NewTimeInSeconds { get; set; }

        /// <summary>
        /// Sau khi đăng thì active được customer ngay
        /// </summary>
        public bool UserIsActiveOnRegister { get; set; }

        /// <summary>
        /// Cho phép đăng ký
        /// </summary>
        public bool RegisterIsEnable { get; set; }

        /// <summary>
        /// Thơi hạn link phục hồi mật khẩu được phép sử dụng tính theo ngày
        /// </summary>
        public int RecoveryPasswordLinkEnableDay { get; set; }

        /// <summary>
        /// Danh sách email quản trị viên được notifi khi người dùng đăng ký. phân cách nhau bằng dấu ,
        /// </summary>
        public string EmailListGiveUserCreated { get; set; }

        /// <summary>
        /// Danh sách email quản trị viên được thông báo khi người dùng liên hệ. phân cách nhau bằng dấu ,
        /// </summary>
        public string EmailListGiveContatsCreated { get; set; }

        /// <summary>
        /// Facebook App Id
        /// </summary>
        public string FacebookApplicationId { get; set; }

        /// <summary>
        /// Facebook App Secret
        /// </summary>
        public string FacebookApplicationSecret { get; set; }

        /// <summary>
        /// Google Client Id
        /// </summary>
        public string GoogleClientId { get; set; }

        /// <summary>
        /// Google ClientSecret
        /// </summary>
        public string GoogleClientSecret { get; set; }

        /// <summary>
        /// Microsoft Client Id
        /// </summary>
        public string MicrosoftClientId { get; set; }

        /// <summary>
        /// Microsoft ClientSecret
        /// </summary>
        public string MicrosoftClientSecret { get; set; }

        /// <summary>
        /// Twitter Consumer Key
        /// </summary>
        public string TwitterConsumerKey { get; set; }

        /// <summary>
        /// Twitter Consumer Secret
        /// </summary>
        public string TwitterConsumerSecret { get; set; }

        /// <summary>
        /// Cho phép đăng nhập bằng fb
        /// </summary>
        public bool IsLoginByFacebook { get; set; }

        /// <summary>
        /// Cho phép đăng nhập bằng google
        /// </summary>
        public bool IsLoginByGoogle { get; set; }

        /// <summary>
        /// Cho phép đăng nhập bằng Twitter
        /// </summary>
        public bool IsLoginByTwitter { get; set; }

        /// <summary>
        /// Cho phép đăng nhập bằng Microsoft
        /// </summary>
        public bool IsLoginMicrosoft { get; set; }

        /// <summary>
        /// Khóa tài khoản nếu đăng nhập sai nhiều lần
        /// </summary>
        public bool LockoutOnFailure { get; set; }

        /// <summary>
        /// Số lần đăng nhập sai sẽ bị khóa
        /// </summary>
        public int MaxFailedLoginAttempts { get; set; }

        /// <summary>
        ///  Thời gian khóa nếu đăng nhập sai vượt quá số lần quy định
        ///  tính bằng giây
        /// </summary>
        public int DefaultLockoutTimeSpan { get; set; }

        /// <summary>
        /// Tỉ lệ ảnh mặc định của hệ thống
        /// dạng 700x300 đơn vị tính px
        /// </summary>
        public string DefaultAvatarFrameRate { get; set; }

        /// <summary>
        /// Lấy trạng thái đã paser của tỉ lệ ảnh mặc định
        /// </summary>
        public (int W, int H) AvatarFrameRateParser
        {
            get
            {
                if (!string.IsNullOrEmpty(DefaultAvatarFrameRate))
                {
                    string[] frameParser = DefaultAvatarFrameRate.Split("x");
                    return (int.Parse(frameParser[0], CultureInfo.InvariantCulture), int.Parse(frameParser[1], CultureInfo.InvariantCulture));
                }
                return (160, 90);
            }
        }
    }
}