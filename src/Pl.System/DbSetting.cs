using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Settings;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Pl.System
{
    /// <summary>
    /// Dịch vụ lưu cấu hình vào cơ sở dữ liệu
    /// </summary>
    public class DbSetting : ISystemSettings
    {
        #region Load settigs
        /// <summary>
        /// Khóa lưu cấu hình tài lên
        /// </summary>
        private const string UploadSettingPath = "system_setting_uploadjson";

        /// <summary>
        /// Khóa lưu cấu hình url
        /// </summary>
        private const string UrlSettingPath = "system_setting_urljson";

        /// <summary>
        /// Khóa lưu cấu hình soạn thảo
        /// </summary>
        private const string EditorSettingPath = "system_setting_editorjson";

        /// <summary>
        /// Khóa lưu cấu hình Seo
        /// </summary>
        private const string SeoSettingPath = "system_setting_seojson";

        /// <summary>
        /// Khóa lưu cấu hình captcha
        /// </summary>
        private const string CaptchaSettingPath = "system_setting_captchajson";

        /// <summary>
        /// Khóa lưu cấu hình người dùng
        /// </summary>
        private const string UserSettingPath = "system_setting_userjson";

        /// <summary>
        /// Khóa lưu cấu hình chung
        /// </summary>
        private const string CommonSettingPath = "system_setting_commonjson";

        /// <summary>
        /// Connection string to db store setting
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Set options and load all settings in db store
        /// </summary>
        /// <param name="connectionString">Connection string to db store settings</param>
        /// <param name="contentRootPath">Thư mục root host web</param>
        public DbSetting(string connectionString, string contentRootPath)
        {
            _connectionString = connectionString;

            #region Khởi tạo common settings

            CommonSettings _commonSetting = new CommonSettings()
            {
                CmsPageSize = 10,
                EncryptionKey = Guid.NewGuid().ToString(),
                ParentToChildString = " > ",
                DocumentWebsite = "https://philoi.net",
                IsSystemInstalled = false,
                ApplicationVersion = "1.0.0",
                EmailSignature = "<br /> plcms",
                AdminEmailsSystemNotify = "youradmin@gmail.com",
            };
            _common = JsonSerializer.Deserialize<CommonSettings>(GetSettingsOfCreateIfNotExist(JsonSerializer.Serialize(_commonSetting), CommonSettingPath, "Thiết lập common settings cho hệ thống."));

            #endregion Khởi tạo common settings

            #region Khởi tạo upload settings

            UploadSettings _uploadSettings = new UploadSettings()
            {
                DomainImgPath = "/uploaded/images/",
                UploadImgPath = Path.Combine(contentRootPath, "wwwroot\\uploaded\\images"),
                ImageAllowedExtensions = "bmp,gif,jpeg,jpg,png,ico",
                ImageMaxSize = 10485760, //10mb
                ImgResizeList = "",//100,250*300,500

                DomainVideoPath = "/uploaded/videos/",
                UploadVideoPath = Path.Combine(contentRootPath, "wwwroot\\uploaded\\videos"),
                VideoAllowedExtensions = "mp4,flv,avi,mpc,mpeg,mpg",
                VideoMaxSize = 104857600, //100mb

                DomainFilePath = "/uploaded/files/",
                UploadFilePath = Path.Combine(contentRootPath, "wwwroot\\uploaded\\files"),
                FileAllowedExtensions = "7z,doc,docx,gz,gzip,ods,odt,pdf,ppt,pptx,pxd,qt,rar,rtf,swf,tar,tgz,tif,tiff,txt,vsd,xls,xlsx,zip",
                FileMaxSize = 104857600, //10mb

                DomainAudioPath = "/uploaded/audios/",
                UploadAudioPath = Path.Combine(contentRootPath, "wwwroot\\uploaded\\audios"),
                AudioAllowedExtensions = "mp3,mov,mid,wav,wma,wmv",
                AudioMaxSize = 104857600, //10mb

                DefaultImageFrameRate = "790x390"
            };
            _upload = JsonSerializer.Deserialize<UploadSettings>(GetSettingsOfCreateIfNotExist(JsonSerializer.Serialize(_uploadSettings), UploadSettingPath, "Thiết lập upload cho hệ thống."));

            #endregion Khởi tạo upload settings

            #region Khởi tạo url settings

            UrlSettings urlSettings = new UrlSettings()
            {
                CmsDomain = "https://cms.philoi.net/",
                WebDomain = "https://philoi.net/",
                MobileDomain = "https://philoi.net/",
                ApiDomain = "https://qpi.philoi.net/",
                FileNotFoundPage = "/notfound.html",
                ErrorPage = "/error.html",
                AccessDeniedPage = "/accessdenied.html",
                SupportSite = "",

                LoginPage = "/Login",
                LogoutPage = "/Logout",
                PasswordChangerPage = "/ChangePassword",
                ForgotPasswordPage = "/ForgotPassword",
                RegisterPage = "/Register",
                UserActivePage = "/UserActive",
            };
            _url = JsonSerializer.Deserialize<UrlSettings>(GetSettingsOfCreateIfNotExist(JsonSerializer.Serialize(urlSettings), UrlSettingPath, "Thiết lập url cho hệ thống."));

            #endregion Khởi tạo url settings

            #region Khởi tạo editor settings

            EditorSettings _editorSetting = new EditorSettings()
            {
                BaseDir = Path.Combine(contentRootPath, "wwwroot\\uploaded\\editor"),
                BaseUrl = "/uploaded/editor",
                FileAllowedExtensions = "7z,aiff,asf,avi,bmp,csv,doc,docx,fla,flv,gif,gz,gzip,jpeg,jpg,mid,mov,mp3,mp4,mpc,mpeg,mpg,ods,odt,pdf,png,ppt,pptx,pxd,qt,ram,rar,rm,rmi,rmvb,rtf,sdc,sitd,swf,sxc,sxw,tar,tgz,tif,tiff,txt,vsd,wav,wma,wmv,xls,xlsx,zip",
                FileMaxSize = 10485760, //10mb
                FlashAllowedExtensions = "swf,flv",
                FlashMaxSize = 10485760, //10mb
                ImageAllowedExtensions = "bmp,gif,jpeg,jpg,png",
                ImageMaxSize = 10485760, //10mb
                ImageHeightMax = 1200,
                ImageWidthMax = 1600,
                ThumbnailsEnable = true
            };
            _editor = JsonSerializer.Deserialize<EditorSettings>(GetSettingsOfCreateIfNotExist(JsonSerializer.Serialize(_editorSetting), EditorSettingPath, "Thiết lập editor cho hệ thống."));

            #endregion Khởi tạo editor settings

            #region Khởi tạo seo settings

            SeoSettings _seoSettings = new SeoSettings()
            {
                PageTitleSeparator = " | ",
                AllowUnicodeCharsInUrls = false,
                SiteNameIsFist = false,
                EnableSocialNetworking = false,
                FacebookLink = "#",
                TwitterLink = "#",
                GoogleLink = "#",
                YoutubeLink = "#",
                Rss = "/rss",
                FacebookLikeButton = true,
                GoogleAddOneButton = true,
                TwitterButton = true,
                FacebookShareButton = true,
                GoogleShareButton = true,
                TwitterShareButton = true,
                AllShareButton = true,
                EnableTagCloud = true,
                SearchByGoogle = false,
                SiteLogoName = "logo.png",
                SiteIconName = "favicon.ico",
                SiteImageName = "siteimage.jpg",
                GmapUrl = "https://www.google.com/maps/ms?msa=0&amp;msid=214530300839439902003.0004f957442204ea3ea14&amp;hl=vi&amp;ie=UTF8&amp;t=m&amp;z=13&amp;output=embed",
                FooterHtmlAdditional = "",
                HeaderHtmlAdditional = "<meta property=\"fb:admins\" content=\"100003131310461\">" +
                "<meta name=\"geo.placename\" content=\"Ha Noi, Viet Nam\" />" +
                "<meta name=\"geo.region\" content=\"VN-HN\" />" +
                "<meta name=\"geo.position\" content=\"21.030624;105.782431\" />",
                GoogleServiceAccountJson = "",
                GoogleAnalyticViewId = ""
            };
            _seo = JsonSerializer.Deserialize<SeoSettings>(GetSettingsOfCreateIfNotExist(JsonSerializer.Serialize(_seoSettings), SeoSettingPath, "Thiết lập ceo cho hệ thống."));

            #endregion Khởi tạo seo settings

            #region Khởi tạo captcha settings

            CaptchaSettings _captchaSetting = new CaptchaSettings()
            {
                Enabled = false,
                ShowOnLoginPage = true,
                ShowOnRegistrationPage = true,
                SecretKey = "6LczFUAUAAAAANwZ5p5_J941EhemD76_RuW-PX9k",
                Theme = "white",
                Type = "image",
                Size = "normal",
                SiteKey = "6LczFUAUAAAAAGiO0bgV1DLASbFPbYjT-62ZFYO_",
                ShowOnForgotPasswordPage = true,
            };
            _captcha = JsonSerializer.Deserialize<CaptchaSettings>(GetSettingsOfCreateIfNotExist(JsonSerializer.Serialize(_captchaSetting), CaptchaSettingPath, "Thiết lập captcha cho hệ thống."));

            #endregion Khởi tạo captcha settings

            #region Khởi tạo user settings

            UserSettings _userSettings = new UserSettings()
            {
                AvatarResizeList = "",
                UploadAvatarPath = Path.Combine(contentRootPath, "wwwroot\\uploaded\\avatar"),
                DomainAvatarPath = "/uploaded/avatar/",
                NewTimeInSeconds = 60 * 60 * 24 * 7,
                AvatarImageAllowedExtensions = "bmp,gif,jpeg,jpg,png",
                AvatarImageMaxSize = 10485760, //10mb
                UserIsActiveOnRegister = false,
                RegisterIsEnable = true,
                RecoveryPasswordLinkEnableDay = 7,
                EmailListGiveUserCreated = "phivanloi@gmail.com",
                EmailListGiveContatsCreated = "phivanloi@gmail.com",
                FacebookApplicationId = "638979723189534",
                FacebookApplicationSecret = "",
                GoogleClientId = "",
                GoogleClientSecret = "",
                TwitterConsumerKey = "",
                TwitterConsumerSecret = "",
                IsLoginByFacebook = false,
                IsLoginByGoogle = false,
                IsLoginByTwitter = false,
                IsLoginMicrosoft = false,
                LockoutOnFailure = false,
                MicrosoftClientId = "",
                MicrosoftClientSecret = "",
                DefaultLockoutTimeSpan = 1800,
                MaxFailedLoginAttempts = 10,
                DefaultAvatarFrameRate = "790x390"
            };
            _user = JsonSerializer.Deserialize<UserSettings>(GetSettingsOfCreateIfNotExist(JsonSerializer.Serialize(_userSettings), UserSettingPath, "Thiết lập user cho hệ thống."));

            #endregion Khởi tạo user settings
        }

        /// <summary>
        /// Lấy nội dung file thiết lập nếu chưa có thì tạo theo string mặc định chuyền vào
        /// </summary>
        /// <param name="settingJsonStringDefault">string mặc định</param>
        /// <param name="key">Đường dẫn đến file setting</param>
        /// <param name="description">Mô tả cho khóa dữ liệu</param>
        /// <returns>nội dung file setting</returns>
        private string GetSettingsOfCreateIfNotExist(string settingJsonStringDefault, string key, string description = "")
        {
            using SystemDbContext dbContext = new SystemDbContext(new DbContextOptionsBuilder<SystemDbContext>().UseSqlServer(_connectionString).Options);
            SystemValue systemKeyValue = dbContext.SystemValues.FirstOrDefault(q => q.Key == key);
            if (systemKeyValue != null)
            {
                return systemKeyValue.Value;
            }

            systemKeyValue = new SystemValue() { Key = key, Value = settingJsonStringDefault, Description = description };
            dbContext.SystemValues.Add(systemKeyValue);
            dbContext.SaveChanges();
            return settingJsonStringDefault;
        }

        /// <summary>
        /// Set một giá trị theo key và mô tả
        /// </summary>
        /// <param name="key">Khóa</param>
        /// <param name="value">Giá trị</param>
        /// <returns>Nếu đã tồn tại thì sẽ update</returns>
        private bool SetStringByKey(string key, string value)
        {
            using SystemDbContext dbContext = new SystemDbContext(new DbContextOptionsBuilder<SystemDbContext>().UseSqlServer(_connectionString).Options);
            SystemValue systemKeyValue = dbContext.SystemValues.FirstOrDefault(q => q.Key == key);
            if (systemKeyValue != null)
            {
                systemKeyValue.Value = value;
            }
            else
            {
                dbContext.SystemValues.Add(new SystemValue() { Key = key, Value = value });
            }
            return dbContext.SaveChanges() > 0;
        }
        #endregion

        private CommonSettings _common;

        /// <summary>
        /// Thiết lập Common
        /// </summary>
        public CommonSettings Common
        {
            get => _common;

            set
            {
                _common = value;
                SetStringByKey(CommonSettingPath, JsonSerializer.Serialize(value));
            }
        }

        private UserSettings _user;

        /// <summary>
        /// Thiết lập User
        /// </summary>
        public UserSettings User
        {
            get => _user;

            set
            {
                _user = value;
                SetStringByKey(UserSettingPath, JsonSerializer.Serialize(value));
            }
        }

        private CaptchaSettings _captcha;

        /// <summary>
        /// Thiết lập Captchar
        /// </summary>
        public CaptchaSettings Captchar
        {
            get => _captcha;

            set
            {
                _captcha = value;
                SetStringByKey(CaptchaSettingPath, JsonSerializer.Serialize(value));
            }
        }

        private SeoSettings _seo;

        /// <summary>
        /// Thiết lập Seo
        /// </summary>
        public SeoSettings Seo
        {
            get => _seo;

            set
            {
                _seo = value;
                SetStringByKey(SeoSettingPath, JsonSerializer.Serialize(value));
            }
        }

        private EditorSettings _editor;

        /// <summary>
        /// Thiết lập Editor
        /// </summary>
        public EditorSettings Editor
        {
            get => _editor;

            set
            {
                _editor = value;
                SetStringByKey(EditorSettingPath, JsonSerializer.Serialize(value));
            }
        }

        private UrlSettings _url;

        /// <summary>
        /// Thiết lập Url
        /// </summary>
        public UrlSettings Url
        {
            get => _url;

            set
            {
                _url = value;
                SetStringByKey(UrlSettingPath, JsonSerializer.Serialize(value));
            }
        }

        private UploadSettings _upload;

        /// <summary>
        /// Thiết lập Upload
        /// </summary>
        public UploadSettings Upload
        {
            get => _upload;

            set
            {
                _upload = value;
                SetStringByKey(UploadSettingPath, JsonSerializer.Serialize(value));
            }
        }
    }
}