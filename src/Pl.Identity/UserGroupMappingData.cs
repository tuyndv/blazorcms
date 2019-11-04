using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pl.Identity.Data
{
    public class UserGroupMappingData : EfRepository<UserGroupMapping>, IUserGroupMappingData
    {
        public UserGroupMappingData(IdentityDbContext identityDbContext) : base(identityDbContext)
        {

        }

        /// <summary>
        /// Lấy danh sách UserGroupMapping
        /// </summary>
        /// <param name="skip">skip row value</param>
        /// <param name="take">take row value</param>
        /// <param name="userId">Id người dùng</param>
        /// <param name="groupId">Id ngóm</param>
        /// <returns>Danh sách mapping</returns>
        public async Task<IDataSourceResult<UserGroupMapping>> GetUserGroupMappingsAsync(int skip, int take, string userId = null, string groupId = null)
        {
            BaseSpecification<UserGroupMapping> baseSpecification = new BaseSpecification<UserGroupMapping>(q =>
            (string.IsNullOrEmpty(userId) || q.UserId == userId)
            && (string.IsNullOrEmpty(groupId) || q.UserGroupId == groupId));
            baseSpecification.ApplyOrderByDescending(q => q.Id);
            baseSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(baseSpecification);
        }

    }
}