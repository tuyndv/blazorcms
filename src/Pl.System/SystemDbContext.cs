using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Security;
using Pl.EntityFrameworkCore;
using System.Collections.Generic;

namespace Pl.System
{
    public class SystemDbContext : DbContext
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {

        }

        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

        public virtual DbSet<CmsMenu> CmsMenus { get; set; }

        public virtual DbSet<Currency> Currencies { get; set; }

        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<LanguageResource> LanguageResources { get; set; }

        public virtual DbSet<ObjectLocalized> ObjectLocalizeds { get; set; }

        public virtual DbSet<FileResource> ResourceFiles { get; set; }

        public virtual DbSet<FileResourceMapping> ResourceFileMappings { get; set; }

        public virtual DbSet<ScheduleTask> ScheduleTasks { get; set; }

        public virtual DbSet<SystemValue> SystemValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region ActivityLog
            modelBuilder.Entity<ActivityLog>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.UserId).HasMaxLength(36).IsRequired();
                b.Property(p => p.Message).HasMaxLength(512);
                b.Property(p => p.IpAddress).HasMaxLength(64);
                b.Property(p => p.PageUrl).HasMaxLength(1024);
                b.Property(p => p.ObjectJson).HasConversion(new DataProtectedConverter(SystemDataProtection.DataProtector));
                b.HasIndex(q => q.UserId);
                b.HasIndex(q => q.Message);
                b.HasIndex(q => q.CreatedTime);
            });
            #endregion

            #region CmsMenu
            modelBuilder.Entity<CmsMenu>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Position).HasMaxLength(64);
                b.Property(p => p.ParentId).HasMaxLength(36);
                b.Property(p => p.CssClass).HasMaxLength(128);
                b.Property(p => p.Name).HasMaxLength(64).IsRequired();
                b.Property(p => p.Link).HasMaxLength(1024);
                b.Property(p => p.RolesString).HasMaxLength(1024);
                b.Property(p => p.TargetType).HasMaxLength(16);
                b.HasIndex(q => q.Position);
                b.HasIndex(q => q.ParentId);
                b.HasData(GetSeedingData());
            });
            #endregion

            #region Currency
            modelBuilder.Entity<Currency>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Name).HasMaxLength(64).IsRequired();
                b.Property(p => p.CurrencyCode).HasMaxLength(8);
                b.Property(p => p.Rate).HasColumnType("decimal(18,2)");
                b.Property(p => p.Culture).HasMaxLength(8).IsRequired();
                b.Property(p => p.CustomFormatting).HasMaxLength(32);
                b.HasIndex(q => q.Culture);
                b.HasData(
                    new Currency()
                    {
                        Name = "VN Đồng",
                        CurrencyCode = "VND",
                        CustomFormatting = "0,000 VND",
                        Culture = "vi-VN",
                        DisplayOrder = 1,
                        IsPrimaryExchange = false,
                        IsPrimarySystem = true,
                        Published = true,
                        Rate = 20000
                    },
                    new Currency()
                    {
                        Name = "US Dollar",
                        CurrencyCode = "USD",
                        CustomFormatting = "",
                        Culture = "en-US",
                        DisplayOrder = 2,
                        IsPrimaryExchange = true,
                        IsPrimarySystem = false,
                        Published = false,
                        Rate = 1
                    }
                );
            });
            #endregion

            #region Language
            modelBuilder.Entity<Language>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Name).HasMaxLength(64).IsRequired();
                b.Property(p => p.FlagImage).HasMaxLength(64);
                b.Property(p => p.Culture).HasMaxLength(8).IsRequired();
                b.HasIndex(q => q.Culture);
                b.HasData(
                    new Language()
                    {
                        Name = "Việt Nam",
                        DisplayDefault = true,
                        DisplayOrder = 1,
                        FlagImage = "vn.png",
                        Culture = "vi-VN",
                        Published = true
                    },
                    new Language()
                    {
                        Name = "English US",
                        DisplayDefault = false,
                        DisplayOrder = 2,
                        FlagImage = "en.png",
                        Culture = "en-US",
                        Published = false
                    }
                );
            });
            #endregion

            #region LanguageResource
            modelBuilder.Entity<LanguageResource>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Key).HasMaxLength(256).IsRequired();
                b.Property(p => p.Type).HasMaxLength(256).IsRequired();
                b.Property(p => p.Culture).HasMaxLength(8).IsRequired();
                b.HasIndex(q => q.Culture);
                b.HasIndex(q => q.Key);
                b.HasIndex(q => q.Type);
            });
            #endregion

            #region ObjectLocalized
            modelBuilder.Entity<ObjectLocalized>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.PropertyName).HasMaxLength(128).IsRequired();
                b.Property(p => p.ObjectId).HasMaxLength(36);
                b.Property(p => p.LanguageCulture).HasMaxLength(8);
                b.HasIndex(q => q.ObjectType);
                b.HasIndex(q => q.LanguageCulture);
                b.HasIndex(q => q.ObjectId);
            });
            #endregion

            #region ResourceFile
            modelBuilder.Entity<FileResource>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Path).HasMaxLength(256).IsRequired();
                b.Property(p => p.Description).HasMaxLength(1024);
                b.Property(p => p.UserId).HasMaxLength(36).IsRequired();
                b.HasIndex(q => q.Type);
                b.HasIndex(q => q.UserId);
            });
            #endregion

            #region ResourceFileMapping
            modelBuilder.Entity<FileResourceMapping>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Title).HasMaxLength(128);
                b.Property(p => p.Caption).HasMaxLength(512);
                b.Property(p => p.Link).HasMaxLength(1024);
                b.Property(p => p.FileResourceId).HasMaxLength(36).IsRequired();
                b.Property(p => p.ObjectId).HasMaxLength(36).IsRequired();
                b.HasIndex(q => q.FileResourceId);
                b.HasIndex(q => q.ObjectId);
                b.HasIndex(q => q.ObjectType);
            });
            #endregion

            #region ScheduleTask
            modelBuilder.Entity<ScheduleTask>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Name).HasMaxLength(128).IsRequired();
                b.Property(p => p.Type).HasMaxLength(64).IsRequired();
                b.HasData(
                    new ScheduleTask()
                    {
                        Name = "Cố gắng gửi lại những email không gửi được.",
                        Enabled = false,
                        Seconds = 600,
                        StopOnError = false,
                        Type = ScheduleType.TrySendFailedEmail
                    },
                    new ScheduleTask()
                    {
                        Name = "Nạp cache tự động nếu dữ liệu chưa có trong cache",
                        Enabled = false,
                        Seconds = 300,
                        StopOnError = false,
                        Type = ScheduleType.CreateAndRefreshCache
                    },
                    new ScheduleTask()
                    {
                        Name = "Xóa db log hệ thống tự động",
                        Enabled = false,
                        Seconds = 86400,
                        StopOnError = false,
                        Type = ScheduleType.RecurringDeleteLog
                    }
                );
            });
            #endregion

            #region SystemValue
            modelBuilder.Entity<SystemValue>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Key).HasMaxLength(128).IsRequired();
                b.Property(p => p.Value).HasConversion(new DataProtectedConverter(SystemDataProtection.DataProtector));
                b.HasIndex(q => q.Key);
            });
            #endregion

        }

        /// <summary>
        /// Get list cms menu to seeding data
        /// </summary>
        /// <returns>IEnumerable CmsMenu</returns>
        private IEnumerable<CmsMenu> GetSeedingData()
        {
            var cmsMenus = new List<CmsMenu>();

            #region Parent Menu
            var parentMenuSortcut = new CmsMenu()
            {
                Position = "CmsHeader",
                Active = true,
                CssClass = "fa fa-plus-square",
                Name = "Lối tắt",
                Link = "",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.UserCanCreate, PermissionConstants.ResourceFileCanCreate }),
                ParentId = string.Empty,
                DisplayOrder = 1,
                TargetType = "",
            };
            cmsMenus.Add(parentMenuSortcut);

            var userManageMenu = new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-user",
                Name = "Người dùng",
                Link = null,
                RolesString = string.Join(",", new List<string>() {
                        PermissionConstants.UserCanCreate,
                        PermissionConstants.UserCanDelete,
                        PermissionConstants.UserCanRead,
                        PermissionConstants.UserCanUpdate,
                        PermissionConstants.UserGroupCanUpdate,
                        PermissionConstants.UserGroupCanUpdate,
                        PermissionConstants.UserGroupCanUpdate,
                        PermissionConstants.UserGroupCanUpdate,}),
                ParentId = string.Empty,
                DisplayOrder = 2,
                TargetType = "",
            };
            cmsMenus.Add(userManageMenu);

            var resourceFileNamage = new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-hdd-o",
                Name = "Tài nguyên",
                Link = null,
                RolesString = string.Join(",", new List<string>() {
                        PermissionConstants.ResourceFileCanRead,
                        PermissionConstants.ResourceFileCanCreate,
                        PermissionConstants.ResourceFileCanUpdate,
                        PermissionConstants.ResourceFileCanDelete }),
                ParentId = string.Empty,
                DisplayOrder = 3,
                TargetType = "",
            };
            cmsMenus.Add(resourceFileNamage);

            var notification = new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-bullhorn",
                Name = "Thông báo",
                Link = null,
                RolesString = string.Join(",", new List<string>() {
                        PermissionConstants.MessageTemplateManager,
                        PermissionConstants.MessageTemplateCanRead,
                        PermissionConstants.MessageTemplateCanCreate,
                        PermissionConstants.MessageTemplateCanUpdate,
                        PermissionConstants.MessageTemplateCanDelete,
                        PermissionConstants.QueueEmailManager,
                        PermissionConstants.QueueEmailCanRead,
                        PermissionConstants.QueueEmailCanCreate,
                        PermissionConstants.QueueEmailCanUpdate,
                        PermissionConstants.QueueEmailCanDelete}),
                ParentId = string.Empty,
                DisplayOrder = 4,
                TargetType = "",
            };
            cmsMenus.Add(notification);

            var systemSettings = new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-gears",
                Name = "Cấu hình",
                Link = null,
                RolesString = string.Join(",", new List<string>() {
                        PermissionConstants.SystemSettings }),
                ParentId = string.Empty,
                DisplayOrder = 5,
                TargetType = "",
            };
            cmsMenus.Add(systemSettings);
            #endregion

            #region Header menu
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsHeader",
                Active = true,
                CssClass = "fa fa-user",
                Name = "Thêm người dùng (alt+u)",
                Link = "/User/Create",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.UserCanCreate }),
                ParentId = parentMenuSortcut.Id,
                DisplayOrder = 1,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsHeader",
                Active = true,
                CssClass = "fa fa-file",
                Name = "Thêm tập tin (alt+f)",
                Link = "/FileResource/Create",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.UserCanCreate }),
                ParentId = parentMenuSortcut.Id,
                DisplayOrder = 2,
                TargetType = "",
            });
            #endregion

            #region User and Dashboard
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-home",
                Name = "Tổng quan",
                Link = "/Home/Dashboard",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.CmsDashbroad }),
                ParentId = string.Empty,
                DisplayOrder = 1,
                TargetType = ""
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Người dùng",
                Link = "/User/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.UserCanRead }),
                ParentId = userManageMenu.Id,
                DisplayOrder = 1,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Nhóm người dùng",
                Link = "/Usergroup/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.UserCanRead }),
                ParentId = userManageMenu.Id,
                DisplayOrder = 2,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Nhật ký hoạt động",
                Link = "/ActivityLog/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.UserCanRead }),
                ParentId = userManageMenu.Id,
                DisplayOrder = 3,
                TargetType = "",
            });
            #endregion

            #region Resource file
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Thư viện tập tin",
                Link = "/Media/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.ResourceFileCanRead }),
                ParentId = resourceFileNamage.Id,
                DisplayOrder = 1,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Tải lên",
                Link = "/Media/Create",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.ResourceFileCanCreate }),
                ParentId = resourceFileNamage.Id,
                DisplayOrder = 2,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Tải lên nhiều",
                Link = "/Media/PatchCreate",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.ResourceFileCanCreate }),
                ParentId = resourceFileNamage.Id,
                DisplayOrder = 3,
                TargetType = "",
            });
            #endregion

            #region Notification
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Tạo email",
                Link = "/QueuedEmail/Create",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.QueueEmailCanCreate }),
                ParentId = notification.Id,
                DisplayOrder = 1,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Email đợi gửi",
                Link = "/QueuedEmail/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.QueueEmailCanRead }),
                ParentId = notification.Id,
                DisplayOrder = 2,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Mẫu thông báo",
                Link = "/MessageTemplate/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.MessageTemplateCanRead }),
                ParentId = notification.Id,
                DisplayOrder = 3,
                TargetType = "",
            });
            #endregion

            #region SystemSettings
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Quản lý menu",
                Link = "/Menu/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 1,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Tài khoản gửi email",
                Link = "/EmailAccount/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 2,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Ngôn ngữ",
                Link = "/Language/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 3,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Tiền tệ",
                Link = "/Currency/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 4,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Cài đặt chung",
                Link = "/SystemValue/CommonSettingsChange",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 4,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Cài đặt seo",
                Link = "/SystemValue/SeoSettingsChange",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 5,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Cài đặt url",
                Link = "/SystemValue/UrlSettingsChange",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 6,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Cài đặt tải lên",
                Link = "/SystemValue/UploadSettingsChange",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 7,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Cài đặt soạn thảo",
                Link = "/SystemValue/EditorSettingsChange",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 8,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Cài đặt captcha",
                Link = "/SystemValue/CaptchaSettingsChange",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 9,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Cài đặt người dùng",
                Link = "/SystemValue/UserSettingsChange",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 10,
                TargetType = "",
            });
            cmsMenus.Add(new CmsMenu()
            {
                Position = "CmsSiderbar",
                Active = true,
                CssClass = "fa fa-circle-o",
                Name = "Cài đặt chuyên sâu",
                Link = "/SystemValue/List",
                RolesString = string.Join(",", new List<string>() { PermissionConstants.SystemSettings }),
                ParentId = systemSettings.Id,
                DisplayOrder = 10,
                TargetType = "",
            });
            #endregion

            return cmsMenus;
        }
    }
}