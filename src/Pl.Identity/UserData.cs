using Microsoft.EntityFrameworkCore;
using Pl.Core;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pl.Identity.Data
{
    public class UserData : EfRepository<User>, IUserData
    {
        public UserData(IdentityDbContext identityDbContext) : base(identityDbContext)
        {

        }

        public async Task<IDataSourceResult<User>> GetUsersAsync(
            int skip,
            int take,
            string groupId = null,
            string email = null,
            string userName = null,
            string displayName = null,
            bool? active = null,
            Gender? gender = null,
            string languageCulture = null,
            string currencyCulture = null,
            DateTime? fromBirthDay = null,
            DateTime? toBirthDay = null,
            DateTime? fromLastLoginTime = null,
            DateTime? toLastLoginTime = null,
            DateTime? fromCreatedTime = null,
            DateTime? toCreatedTime = null)
        {
            IQueryable<User> userQuery = DbContext.Set<User>().AsNoTracking();
            if (!string.IsNullOrEmpty(groupId))
            {
                IQueryable<UserGroupMapping> mappingQuery = DbContext.Set<UserGroupMapping>().Where(q => q.UserGroupId == groupId);
                userQuery = from user in userQuery
                            join mapping in mappingQuery on user.Id equals mapping.UserId
                            where mapping.UserGroupId == groupId
                            select user;
            }

            BaseSpecification<User> baseSpecification = new BaseSpecification<User>(q =>
            !q.Deleted
            && (string.IsNullOrEmpty(userName) || q.UserName.Contains(userName))
            && (string.IsNullOrEmpty(displayName) || q.DisplayName.Contains(displayName))
            && (string.IsNullOrEmpty(email) || q.Email == email)
            && (string.IsNullOrEmpty(languageCulture) || q.LanguageCulture == languageCulture)
            && (string.IsNullOrEmpty(currencyCulture) || q.CurrencyCode == currencyCulture)
            && (!active.HasValue || q.Active == active)
            && (!gender.HasValue || q.Gender == gender)
            && (!fromBirthDay.HasValue || q.BirthDay >= fromBirthDay)
            && (!toBirthDay.HasValue || q.BirthDay <= toBirthDay)
            && (!fromLastLoginTime.HasValue || q.LastLoginTime >= fromLastLoginTime)
            && (!toLastLoginTime.HasValue || q.LastLoginTime <= toLastLoginTime)
            && (!fromCreatedTime.HasValue || q.CreatedTime >= fromCreatedTime)
            && (!toCreatedTime.HasValue || q.CreatedTime <= toCreatedTime));
            baseSpecification.ApplyOrderByDescending(q => q.CreatedTime);
            baseSpecification.ApplySelector(x => new User
            {
                Id = x.Id,
                AvatarImage = x.AvatarImage,
                Email = x.Email,
                DisplayName = x.DisplayName,
                Active = x.Active,
                CreatedTime = x.CreatedTime,
                UpdatedTime = x.UpdatedTime,
                LastLoginTime = x.LastLoginTime
            });
            baseSpecification.ApplyPaging(skip, take);

            return await userQuery.ToDataSourceResultAsync(baseSpecification);
        }

        public async Task<string> GetInfoByUserIdAsync(string userId, bool isEmail = false)
        {
            var user = await FindAsync(q => q.Id == userId);
            return user != null ? isEmail ? user.Email : user.DisplayName : string.Empty;
        }

    }
}