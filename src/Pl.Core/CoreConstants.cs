using Microsoft.Extensions.Logging;
using Pl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Pl.Core
{
    public enum Gender
    {
        /// <summary>
        /// Nam
        /// </summary>
        Male,

        /// <summary>
        /// Nữ
        /// </summary>
        Female,

        /// <summary>
        /// Khác
        /// </summary>
        Other
    }

    public enum ObjectTypeEnum
    {
        /// <summary>
        /// Hệ thống
        /// </summary>
        System,

        /// <summary>
        /// Người dùng
        /// </summary>
        User,

        /// <summary>
        /// Nhóm người dùng
        /// </summary>
        UserGroup,

        /// <summary>
        /// Liên kết nhóm người dùng và người dùng
        /// </summary>
        UserGroupMapping,

        /// <summary>
        /// Nhật ký hoạt động
        /// </summary>
        ActivityLog,

        /// <summary>
        /// Nhật ký lỗi
        /// </summary>
        ErrorLog,

        /// <summary>
        /// Giá trị ngôn ngữ
        /// </summary>
        ObjectLocalized,

        /// <summary>
        /// Tác vụ nền
        /// </summary>
        ScheduleTask,

        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        Language,

        /// <summary>
        /// Giá trị ngôn ngữ
        /// </summary>
        LanguageResource,

        /// <summary>
        /// Tiền tệ
        /// </summary>
        Currency,

        /// <summary>
        /// Cấu hình hệ thống
        /// </summary>
        SystemValue,

        /// <summary>
        /// Tài khoản Email
        /// </summary>
        EmailAccount,

        /// <summary>
        /// Mẫu tin nhắn
        /// </summary>
        MessageTemplate,

        /// <summary>
        /// Email đợi gửi
        /// </summary>
        QueuedEmail,

        /// <summary>
        /// Tập tin
        /// </summary>
        Media,

        /// <summary>
        /// Liên kết tập tin và đối tượng
        /// </summary>
        ResourceFileMapping,

        /// <summary>
        /// cms menu object
        /// </summary>
        CmsMenu,
    }

    public static class CoreConstants
    {
        #region Activitylog
        /// <summary>
        /// List Activity Constants with name
        /// </summary>
        public static readonly Dictionary<ActivityTypeEnum, string> ActivityTypes = new Dictionary<ActivityTypeEnum, string>() {
            { ActivityTypeEnum.System, "Hệ thống" },
            { ActivityTypeEnum.Restart, "Khởi động lại" },
            { ActivityTypeEnum.ClearCache, "Xóa cache" },
            { ActivityTypeEnum.Insert, "Thêm mới" },
            { ActivityTypeEnum.Update, "Sửa" },
            { ActivityTypeEnum.Delete, "Xóa" },
            { ActivityTypeEnum.Login, "Đăng nhập" },
            { ActivityTypeEnum.Logout, "Đăng xuất" },
            { ActivityTypeEnum.ViewList, "Xem danh sách" },
            { ActivityTypeEnum.ViewDetails, "Xem chi tiết" },
            { ActivityTypeEnum.ChangePassword, "Đổi mật khẩu" },
            { ActivityTypeEnum.Register, "Đăng ký" },
            { ActivityTypeEnum.UserActive, "Kích hoạt người dùng" },
            { ActivityTypeEnum.PasswordRecovery, "Yêu cầu đổi mật khẩu" },
            { ActivityTypeEnum.UserProfile, "Đổi thông tin cá nhân" },
            { ActivityTypeEnum.Import, "Nhập dữ liệu" },
            { ActivityTypeEnum.Export, "Xuất dữ liệu" },
            { ActivityTypeEnum.SetDefaultLanguage, "Đặt mặc định ngôn ngữ" },
            { ActivityTypeEnum.SetDefaultCurrency, "Đặt mặc định tiền tệ" }
        };

        #endregion

        #region Common
        /// <summary>
        /// Khóa này dùng để cộng với các cache tag helper. và có thể đổi lúc runtime
        /// </summary>
        public static string DefaultViewCacheKey { get; set; } = DateTime.Now.ToString("ddMMyyyyhhmmssfff", CultureInfo.InvariantCulture);

        /// <summary>
        /// Thời gian cache mặc định tính theo giây
        /// </summary>
        public const int DefaultCacheTime = 60;

        /// <summary>
        /// Thời gian cache mặc định của view action mặc định tính bằng giây
        /// </summary>
        public const int DefaultResponseCacheTime = 3600;

        /// <summary>
        /// Các request gửi đi sẽ thêm phần user agent này
        /// </summary>
        public const string RequestUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36";
        #endregion

        #region CacheKey
        public static readonly string UserCacheKey = $"{ObjectTypeEnum.User}_userck_";
        public static readonly string EmailAccountCacheKey = $"{ObjectTypeEnum.EmailAccount}_eck";
        public static readonly string MessageTemplateCacheKey = $"{ObjectTypeEnum.EmailAccount}_meck";
        public static readonly string ResourceFileCacheKey = $"{ObjectTypeEnum.Media}_mdack";
        public static readonly string CmsMenuCacheKey = $"{ObjectTypeEnum.CmsMenu}_menuck";
        public static readonly string CurrencyCacheKey = $"{ObjectTypeEnum.Currency}_crck";
        public static readonly string LanguageCacheKey = $"{ObjectTypeEnum.Language}_lck";
        #endregion

        #region ErrorLog
        /// <summary>
        /// List error log type with name
        /// </summary>
        public static readonly Dictionary<LogLevel, string> ErrorLogLevels = new Dictionary<LogLevel, string>() {
            {LogLevel.Critical, "Lỗi dừng hệ thống" },
            {LogLevel.Debug, "Gỡ lỗi" },
            {LogLevel.Error, "Lỗi tiến trình" },
            {LogLevel.Information, "Thông tin" },
            {LogLevel.None, "Lưu vết thông tin" },
            {LogLevel.Trace, "Theo dõi" },
            {LogLevel.Warning, "Cảnh báo" }
        };
        #endregion

        #region ResourceFile
        /// <summary>
        /// List resource type with name
        /// </summary>
        public static readonly Dictionary<ResourceType, string> ResourceTypes = new Dictionary<ResourceType, string>() {
            {ResourceType.None, "Tập tin thường" },
            {ResourceType.Avatar, "Tập tin ảnh đại diện" },
            {ResourceType.Image, "Tập tin ảnh" },
            {ResourceType.Movie, "Tập tin phim" },
            {ResourceType.Audio, "Tập tin âm thanh" }
        };
        #endregion

        #region ObjectType
        /// <summary>
        /// List object type with name
        /// </summary>
        public static readonly Dictionary<ObjectTypeEnum, string> ObjectTypes = new Dictionary<ObjectTypeEnum, string>() {
            {ObjectTypeEnum.System, "Hệ thống" },
            {ObjectTypeEnum.User, "Người dùng" },
            {ObjectTypeEnum.UserGroup, "Nhóm người dùng" },
            {ObjectTypeEnum.UserGroupMapping, "Liên kết nhóm người dùng và người dùng" },
            {ObjectTypeEnum.ActivityLog, "Nhật ký hoạt động" },
            {ObjectTypeEnum.ErrorLog, "Nhật ký lỗi" },
            {ObjectTypeEnum.ObjectLocalized, "Giá trị ngôn ngữ của đối tượng" },
            {ObjectTypeEnum.ScheduleTask, "Tác vụ nền" },
            {ObjectTypeEnum.Language, "Ngôn ngữ" },
            {ObjectTypeEnum.LanguageResource, "Giá trị ngôn ngữ hiển thị" },
            {ObjectTypeEnum.Currency, "Tiền tệ" },
            {ObjectTypeEnum.SystemValue, "Cấu hình hệ thống" },
            {ObjectTypeEnum.EmailAccount, "Tài khoản email" },
            {ObjectTypeEnum.MessageTemplate, "Mẫu tin nhắn" },
            {ObjectTypeEnum.QueuedEmail, "Email đợi gửi" },
            {ObjectTypeEnum.Media, "Tập tin" },
            {ObjectTypeEnum.ResourceFileMapping, "Liên kết tập tin và đối tượng" },
            {ObjectTypeEnum.CmsMenu, "Menu" }
        };
        #endregion

        #region User
        /// <summary>
        /// List gender type with name
        /// </summary>
        public static readonly Dictionary<Gender, string> Genders = new Dictionary<Gender, string>() {
            {Gender.Male, "Nam" },
            {Gender.Female, "Nữ" },
            {Gender.Other, "Khác" }
        };
        #endregion
    }
}
