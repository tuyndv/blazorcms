using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IUserGroupData : IAsyncRepository<UserGroup>
    {
        /// <summary>
        /// Get list user group
        /// </summary>
        /// <param name="skip">Skip row value</param>
        /// <param name="take">Take row value</param>
        /// <param name="name">group name to find</param>
        Task<IDataSourceResult<UserGroup>> GetUserGroupsAsync(int skip, int take, string name = "");
    }
}