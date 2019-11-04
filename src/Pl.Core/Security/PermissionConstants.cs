using System.Collections.Generic;

namespace Pl.Core.Security
{
    public static class PermissionConstants
    {
        #region System
        /// <summary>
        /// Access to cms
        /// </summary>
        public const string CmsDashbroad = "st-cd";

        /// <summary>
        /// System manager
        /// </summary>
        public const string SystemManager = "st-sm";

        /// <summary>
        /// Clear cache
        /// </summary>
        public const string SystemClearCache = "st-cc";

        /// <summary>
        /// System restart
        /// </summary>
        public const string SystemRestart = "st-rs";

        /// <summary>
        /// System log view
        /// </summary>
        public const string SystemLogView = "st-lv";

        /// <summary>
        /// Activity system log view
        /// </summary>
        public const string SystemActivityLogView = "st-alv";

        /// <summary>
        /// View system info and maintenance tool
        /// </summary>
        public const string SystemInfoAndMaintenace = "st-imt";

        /// <summary>
        /// Schedule manager
        /// </summary>
        public const string SystemSchedule = "st-slm";

        /// <summary>
        /// System settings
        /// </summary>
        public const string SystemSettings = "st-sst";
        #endregion

        #region Resource File
        /// <summary>
        /// Role key for manager resource file
        /// </summary>
        public const string ResourceFileManager = "rf-mg";

        /// <summary>
        /// Read resource file
        /// </summary>
        public const string ResourceFileCanRead = "rf-cr";

        /// <summary>
        /// Create resource file
        /// </summary>
        public const string ResourceFileCanCreate = "rf-ce";

        /// <summary>
        /// update resource file
        /// </summary>
        public const string ResourceFileCanUpdate = "rf-up";

        /// <summary>
        /// Delete resource file
        /// </summary>
        public const string ResourceFileCanDelete = "rf-dl";
        #endregion

        #region User
        /// <summary>
        /// Role key for manager system user
        /// </summary>
        public const string UserManager = "us-mg";

        /// <summary>
        /// Read user
        /// </summary>
        public const string UserCanRead = "us-cr";

        /// <summary>
        /// Create user
        /// </summary>
        public const string UserCanCreate = "us-ce";

        /// <summary>
        /// Update user
        /// </summary>
        public const string UserCanUpdate = "us-up";

        /// <summary>
        /// Delete user
        /// </summary>
        public const string UserCanDelete = "us-dl";
        #endregion

        #region UserGroup
        /// <summary>
        /// Role key for manager system user
        /// </summary>
        public const string UserGroupManager = "ug-mg";

        /// <summary>
        /// Read user group
        /// </summary>
        public const string UserGroupCanRead = "ug-cr";

        /// <summary>
        /// Create user group
        /// </summary>
        public const string UserGroupCanCreate = "ug-ce";

        /// <summary>
        /// Update user group
        /// </summary>
        public const string UserGroupCanUpdate = "ug-up";

        /// <summary>
        /// Delete user group
        /// </summary>
        public const string UserGroupCanDelete = "ug-dl";
        #endregion

        #region QueueEmail
        /// <summary>
        /// Role key for manager queue email
        /// </summary>
        public const string QueueEmailManager = "qe-mg";

        /// <summary>
        /// Read queue email
        /// </summary>
        public const string QueueEmailCanRead = "qe-cr";

        /// <summary>
        /// Create queue email
        /// </summary>
        public const string QueueEmailCanCreate = "qe-ce";

        /// <summary>
        /// update queue email
        /// </summary>
        public const string QueueEmailCanUpdate = "qe-up";

        /// <summary>
        /// Delete queue email
        /// </summary>
        public const string QueueEmailCanDelete = "qe-dl";
        #endregion

        #region MessageTemplate
        /// <summary>
        /// Role key for manager message template
        /// </summary>
        public const string MessageTemplateManager = "mt-mg";

        /// <summary>
        /// Read message template
        /// </summary>
        public const string MessageTemplateCanRead = "mt-cr";

        /// <summary>
        /// Create message template
        /// </summary>
        public const string MessageTemplateCanCreate = "mt-ce";

        /// <summary>
        /// update message template
        /// </summary>
        public const string MessageTemplateCanUpdate = "mt-up";

        /// <summary>
        /// Delete message template
        /// </summary>
        public const string MessageTemplateCanDelete = "mt-dl";
        #endregion

        /// <summary>
        /// Get administrator role in system
        /// </summary>
        public static readonly IEnumerable<Permission> AdministratorRoles = new List<Permission>() {
            new Permission(CmsDashbroad, "Truy cập cms"),

            new Permission(SystemManager, "Quản trị hệ thống", new List<Permission>(){
                new Permission(SystemClearCache, "Xóa cache hệ thống"),
                new Permission(SystemRestart, "Khởi động lại hệ thống"),
                new Permission(SystemLogView, "Xem log  hệ thống"),
                new Permission(SystemActivityLogView, "Xem nhật ký người dùng"),
                new Permission(SystemInfoAndMaintenace, "Xem thông tin và bào trì hệ thống"),
                new Permission(SystemSchedule, "Quản lý tác vụ nền"),
                new Permission(SystemSettings, "Cấu hình hệ thống"),
            }),

            new Permission(UserManager, "Quản lý thành viên", new List<Permission>(){
                new Permission(UserCanRead, "Xem danh sách thành viên"),
                new Permission(UserCanCreate, "Thêm mới thành viên"),
                new Permission(UserCanUpdate, "Sửa thông tin thành viên"),
                new Permission(UserCanDelete, "Xóa, chặn thành viên")
            }),

            new Permission(UserGroupManager, "Quản lý nhóm thành viên", new List<Permission>(){
                new Permission(UserGroupCanRead, "Xem danh sách nhóm thành viên"),
                new Permission(UserGroupCanCreate, "Thêm mới nhóm thành viên"),
                new Permission(UserGroupCanUpdate, "Sửa thông tin nhóm thành viên"),
                new Permission(UserGroupCanDelete, "Xóa, nhóm thành viên")
            }),

            new Permission(ResourceFileManager, "Quản lý tập tin", new List<Permission>(){
                new Permission(ResourceFileCanRead, "Xem tập tin"),
                new Permission(ResourceFileCanCreate, "Tạo tập tin"),
                new Permission(ResourceFileCanUpdate, "Sửa tập tin"),
                new Permission(ResourceFileCanDelete, "Xóa tập tin")
            }),

            new Permission(QueueEmailManager, "Quản lý email đợi gửi", new List<Permission>(){
                new Permission(QueueEmailCanRead, "Xem email đợi gửi"),
                new Permission(QueueEmailCanCreate, "Tạo email đợi gửi"),
                new Permission(QueueEmailCanUpdate, "Sửa email đợi gửi"),
                new Permission(QueueEmailCanDelete, "Xóa email đợi gửi")
            }),

            new Permission(MessageTemplateManager, "Quản lý tài khoản gửi email", new List<Permission>(){
                new Permission(MessageTemplateCanRead, "Xem tài khoản gửi email"),
                new Permission(MessageTemplateCanCreate, "Tạo tài khoản gửi email"),
                new Permission(MessageTemplateCanUpdate, "Sửa tài khoản gửi email"),
                new Permission(MessageTemplateCanDelete, "Xóa tài khoản gửi email")
            })
        };

        /// <summary>
        /// Get register role in system
        /// </summary>
        public static readonly IEnumerable<Permission> RegisterRoles = new List<Permission>()
        {

        };
    }
}
