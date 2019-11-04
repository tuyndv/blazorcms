using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;

namespace Pl.Identity
{
    public class IdentityDbContext : IdentityDbContext<User>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }

        public virtual DbSet<UserGroup> UserGroups { get; set; }

        public virtual DbSet<UserGroupMapping> UserGroupMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.AvatarImage).HasMaxLength(512);
                b.Property(p => p.DisplayName).HasMaxLength(64).IsRequired();
                b.Property(p => p.Company).HasMaxLength(256);
                b.Property(p => p.RolesString).HasMaxLength(1024);
                b.Property(p => p.SocialProfile.Website).HasMaxLength(1024);
                b.Property(p => p.SocialProfile.Twitter).HasMaxLength(1024);
                b.Property(p => p.SocialProfile.Facebook).HasMaxLength(1024);
                b.Property(p => p.SocialProfile.YouTube).HasMaxLength(1024);
                b.Property(p => p.SocialProfile.Intrargam).HasMaxLength(1024);
                b.Property(p => p.SocialProfile.Skype).HasMaxLength(1024);
                b.ToTable("Users");
                b.OwnsOne(o => o.SocialProfile);
            });

            builder.Entity<UserGroup>(b =>
            {
                b.Property(p => p.Name).HasMaxLength(64).IsRequired();
                b.Property(p => p.RolesString).HasMaxLength(1024);
                b.Property(p => p.Name).HasMaxLength(64).IsRequired();
            });

            builder.Entity<UserGroupMapping>(b =>
            {
                b.Property(p => p.UserId).HasMaxLength(36).IsRequired();
                b.Property(p => p.UserGroupId).HasMaxLength(36).IsRequired();
                b.HasIndex(p => p.UserId);
                b.HasIndex(p => p.UserGroupId);
            });

            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        }
    }
}