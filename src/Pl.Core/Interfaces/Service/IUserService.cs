using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Get cache user by id
        /// </summary>
        /// <param name="userId">Id of user to get</param>
        /// <returns>User</returns>
        Task<User> CacheGetUserById(string userId);
    }
}
