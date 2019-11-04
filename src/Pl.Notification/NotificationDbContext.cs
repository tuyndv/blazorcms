using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;

namespace Pl.Notification
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<QueuedEmail> QueuedEmails { get; set; }

        public virtual DbSet<EmailAccount> EmailAccounts { get; set; }

        public virtual DbSet<MessageTemplate> MessageTemplates { get; set; }

        public virtual DbSet<UserMessage> UserMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region QueuedEmail
            modelBuilder.Entity<QueuedEmail>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.EmailAccountId).HasMaxLength(36);
                b.Property(p => p.To).HasMaxLength(256).IsRequired();
                b.Property(p => p.ToName).HasMaxLength(64);
                b.Property(p => p.Title).HasMaxLength(128).IsRequired();
                b.Property(p => p.EmailBody).IsRequired();
                b.HasIndex(q => q.SendTime);
                b.HasIndex(q => q.EmailAccountId);
                b.HasIndex(q => q.To);
            });
            #endregion

            #region EmailAccount
            modelBuilder.Entity<EmailAccount>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Email).HasMaxLength(256).IsRequired();
                b.Property(p => p.Name).HasMaxLength(64);
                b.Property(p => p.Host).HasMaxLength(128).IsRequired();
                b.Property(p => p.UserName).HasMaxLength(128).IsRequired();
                b.Property(p => p.Password).HasMaxLength(1024).IsRequired();
                b.Property(p => p.DaySendLimited).HasMaxLength(16);
                b.HasIndex(q => q.UseDefault);
                b.HasIndex(q => q.DaySendLimited);
            });
            #endregion

            #region MessageTemplate
            modelBuilder.Entity<MessageTemplate>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Name).HasMaxLength(128).IsRequired();
                b.Property(p => p.Title).HasMaxLength(128).IsRequired();
                b.Property(p => p.Body).IsRequired();
                b.HasIndex(q => q.Name);
                b.HasData(
                    new MessageTemplate()
                    {
                        Name = SystemTemplateName.WelcomeToSystem.ToString(),
                        Body = $"Chào mừng [userfullname] đã đến với pl cms. <br /> Hiện tại bạn đã có thể đăng nhập và sử dụng một số dịch vụ của chúng tôi.",
                        Title = $"Xin chào [useremail] đăng ký đã hoàn tất.",
                        Description = "[useremail] => Email của người dùng. <br /> [userfullname] => tên đầy đủ của người dùng. <br /> [userpassword] => Mật khẩu của người dùng. <br /> [callbacklink] => Đường dẫn của hệ thống để xác nhận người dùng đã đọc mail. <br />",
                        IsSystemTemplate = true
                    },
                    new MessageTemplate()
                    {
                        Name = SystemTemplateName.RegisterActiveByEmail.ToString(),
                        Body = $"Xin chào [userfullname] bạn còn một bước nữa để kích hoạt tài khoản của mình bằng cách bấm vào link được đây hoặc copy và paste vào trình duyệt. <br /> <a href=\"[activelink]\">[activelink]</a>.",
                        Title = $"Kích hoạt tài khoản tại [systemname] đề hoàn tất đăng ký.",
                        Description = "[systemname] => Tên hệ thống, website. <br /> [userfullname] => tên đầy đủ của người dùng. <br /> [activelink] => Đường dẫn kích hoạt tài khoản của người dùng. <br />",
                        IsSystemTemplate = true
                    },
                    new MessageTemplate()
                    {
                        Name = SystemTemplateName.ForgotPassword.ToString(),
                        Body = $"Xin chào [userfullname] bạn có thể đặt lại mật khẩu cho tài khoản của mình bằng cách bấm vào link được đây hoặc copy và paste vào trình duyệt. <br /> <a href=\"[recoverylink]\">[recoverylink]</a>.",
                        Title = $"Lấy lại mật khẩu tại [systemname].",
                        Description = "[systemname] => Tên hệ thống, website. <br /> [userfullname] => tên đầy đủ của người dùng. <br /> [recoverylink] => Đường dẫn đặt lại mật khẩu. <br />",
                        IsSystemTemplate = true
                    }
                );
            });
            #endregion

            #region UserMessage
            modelBuilder.Entity<UserMessage>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36).ValueGeneratedOnAdd();
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.ReceiverUserId).HasMaxLength(36).IsRequired();
                b.Property(p => p.SendUserId).HasMaxLength(36);
                b.Property(p => p.Icon).HasMaxLength(64);
                b.Property(p => p.Message).HasMaxLength(512);
                b.Property(p => p.Link).HasMaxLength(1024);
                b.HasIndex(q => q.ReceiverUserId);
            });
            #endregion
        }
    }
}