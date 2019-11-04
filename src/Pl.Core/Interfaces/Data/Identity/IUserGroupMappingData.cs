using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IUserGroupMappingData : IAsyncRepository<UserGroupMapping>
    {

        /// <summary>
        /// Lấy danh sách UserGroupMapping
        /// </summary>
        /// <param name="skip">skip row value</param>
        /// <param name="take">take row value</param>
        /// <param name="userId">Id người dùng</param>
        /// <param name="groupId">Id ngóm</param>
        /// <returns>Danh sách mapping</returns>
        Task<IDataSourceResult<UserGroupMapping>> GetUserGroupMappingsAsync(int skip, int take, string userId = null, string groupId = null);

    }
}