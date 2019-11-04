using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using System.Threading.Tasks;

namespace Pl.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncCacheService _asyncCacheService;
        private readonly IUserData _userData;

        public UserService(IAsyncCacheService asyncCacheService, IUserData userData)
        {
            _asyncCacheService = asyncCacheService;
            _userData = userData;
        }

        public virtual async Task<User> CacheGetUserById(string userId)
        {
            GuardClausesParameter.NullOrEmpty(userId, nameof(userId));

            string cacheKey = $"{CoreConstants.UserCacheKey}gcua_{userId}uid";
            return await _asyncCacheService.GetOrCreateAsync(cacheKey, async () => await _userData.FindAsync(userId), CoreConstants.DefaultCacheTime);
        }
    }
}
