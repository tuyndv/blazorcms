using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pl.Identity.Data
{
    public class UserGroupData : EfRepository<UserGroup>, IUserGroupData
    {
        public UserGroupData(IdentityDbContext identityDbContext) : base(identityDbContext)
        {

        }

        public async Task<IDataSourceResult<UserGroup>> GetUserGroupsAsync(int skip, int take, string name = "")
        {
            BaseSpecification<UserGroup> baseSpecification = new BaseSpecification<UserGroup>(q =>
            (string.IsNullOrEmpty(name) || q.Name.Contains(name)));
            baseSpecification.ApplyOrderBy(q => q.DisplayOrder);
            baseSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(baseSpecification);
        }
    }
}