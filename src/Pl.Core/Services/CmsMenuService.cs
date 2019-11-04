using Pl.Core.Entities;
using Pl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pl.Core.Services
{
    public class CmsMenuService : ICmsMenuService
    {
        #region Properties And Constructor

        private readonly ICmsMenuData _cmsMenuData;
        private readonly IAsyncCacheService _asyncCacheService;

        public CmsMenuService(
            IAsyncCacheService asyncCacheService,
            ICmsMenuData cmsMenuData)
        {
            _cmsMenuData = cmsMenuData;
            _asyncCacheService = asyncCacheService;
        }

        #endregion Properties And Constructor

        public virtual async Task<IReadOnlyList<CmsMenu>> GetMenusByRolesAsync(List<string> roles)
        {
            var listAllMenus = await _cmsMenuData.FindAllNoTrackingAsync(q => q.Active);
            return listAllMenus.Where(q => q.RolesStringArray.Intersect(roles).Any()).ToList();
        }

        public virtual async Task<IEnumerable<TreeItem<CmsMenu>>> CacheGetByPositionAsync(string position)
        {
            string cacheKey = $"{CoreConstants.CmsMenuCacheKey}_agbp_cid{position}";
            return await _asyncCacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                var query = await _cmsMenuData.FindAllNoTrackingAsync();
                if (!string.IsNullOrEmpty(position))
                {
                    query = query.Where(q => q.Position.Contains(position, StringComparison.InvariantCulture)).ToList();
                }
                return query.ToList().GenerateTree(q => q.Id, q => q.ParentId, q => q.DisplayOrder, string.Empty);
            }, CoreConstants.DefaultCacheTime);
        }

        public virtual async Task<IReadOnlyList<CmsMenu>> CacheGetAllAsync()
        {
            string cacheKey = $"{CoreConstants.CmsMenuCacheKey}_cachegetall";
            return await _asyncCacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                return await _cmsMenuData.FindAllNoTrackingAsync();
            }, CoreConstants.DefaultCacheTime);
        }

        public virtual async Task<string> GetTreeNameAsync(string childId, string separater = " > ")
        {
            List<CmsMenu> listTree = GetTreeByChild(await CacheGetAllAsync(), childId, new List<CmsMenu>());
            return listTree?.Count > 0 ? string.Join(separater, listTree.Select(q => q.Name).ToList()) : string.Empty;

            List<CmsMenu> GetTreeByChild(IReadOnlyList<CmsMenu> cmsMenus, string childId, List<CmsMenu> tree)
            {
                CmsMenu child = cmsMenus.FirstOrDefault(q => q.Id == childId);
                if (child != null && !tree.Any(q => q.Id == child.Id))
                {
                    tree.Add(child);
                    if (!string.IsNullOrEmpty(child.ParentId))
                    {
                        return GetTreeByChild(cmsMenus, child.ParentId, tree);
                    }
                }
                return tree.Reverse<CmsMenu>().ToList();
            }
        }
    }
}