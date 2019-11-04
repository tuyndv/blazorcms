using Microsoft.EntityFrameworkCore;
using Pl.Core;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using Pl.Core.Exceptions;
using Pl.EntityFrameworkCore;

namespace Pl.System
{
    public class CmsMenuData : EfRepository<CmsMenu>, ICmsMenuData
    {
        public CmsMenuData(SystemDbContext systemDbContext) : base(systemDbContext)
        {

        }

        public async Task<IDataSourceResult<CmsMenu>> GetMenusAsync(int skip, int take, string position = "", bool? active = null, string name = null, string parentId = null)
        {
            BaseSpecification<CmsMenu> menuSpecification = new BaseSpecification<CmsMenu>(q =>
            (string.IsNullOrEmpty(name) || EF.Functions.Contains(q.Name, name))
            && (string.IsNullOrEmpty(position) || EF.Functions.Contains(q.Position, position))
            && (string.IsNullOrEmpty(parentId) || q.ParentId == parentId)
            && (!active.HasValue || q.Active == active));
            menuSpecification.ApplyOrderBy(q => q.DisplayOrder);
            menuSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(menuSpecification);
        }

        public async Task<IEnumerable<TreeItem<CmsMenu>>> GetMenusAsync(string position = "", bool? active = null, string parentId = null, string excludeId = null)
        {
            BaseSpecification<CmsMenu> baseSpecification = new BaseSpecification<CmsMenu>(q =>
            (string.IsNullOrEmpty(position) || q.Position == position)
            && (string.IsNullOrEmpty(position) || q.ParentId == parentId)
            && (string.IsNullOrEmpty(excludeId) || q.Id != excludeId)
            && (!active.HasValue || q.Active == active));
            baseSpecification.ApplyOrderByDescending(q => q.Id);
            baseSpecification.ApplySelector(q => new CmsMenu()
            {
                Id = q.Id,
                Name = q.Name,
                ParentId = q.ParentId,
                DisplayOrder = q.DisplayOrder
            });
            var menus = await FindAllNoTrackingAsync(baseSpecification);
            return menus.GenerateTree(q => q.Id, q => q.ParentId, q => q.DisplayOrder, string.Empty);
        }

        public async Task<string> ExportToJsonAsync()
        {
            return JsonSerializer.Serialize((await FindAllNoTrackingAsync()).OrderBy(q => q.Id).Select(q => new
            {
                q.Id,
                q.Position,
                q.Active,
                q.CssClass,
                q.Name,
                q.Link,
                q.RolesString,
                q.ParentId,
                q.DisplayOrder,
                q.TargetType
            }), new JsonSerializerOptions() { WriteIndented = true });
        }

        public async Task<bool> ImportFromJsonAsync(string json)
        {
            GuardClausesParameter.NullOrEmpty(json, nameof(json));
            List<CmsMenu> listMenu = JsonSerializer.Deserialize<List<CmsMenu>>(json);
            IEnumerable<TreeItem<CmsMenu>> treeMenu = listMenu.GenerateTree(q => q.Id, q => q.ParentId, q => q.DisplayOrder, string.Empty);
            await CreateMenuAsync(treeMenu, string.Empty);
            return true;

            async Task CreateMenuAsync(IEnumerable<TreeItem<CmsMenu>> listTreeMenu, string parentId)
            {
                foreach (TreeItem<CmsMenu> cMenu in listTreeMenu)
                {
                    CmsMenu menuInsert = new CmsMenu()
                    {
                        Active = cMenu.Item.Active,
                        DisplayOrder = cMenu.Item.DisplayOrder,
                        CssClass = cMenu.Item.CssClass,
                        Link = cMenu.Item.Link,
                        Name = cMenu.Item.Name,
                        ParentId = parentId,
                        Position = cMenu.Item.Position,
                        RolesString = cMenu.Item.RolesString,
                        TargetType = cMenu.Item.TargetType
                    };
                    bool checkInsert = await InsertAsync(menuInsert);
                    if (checkInsert)
                    {
                        await CreateMenuAsync(cMenu.Children, menuInsert.Id);
                    }
                }
            }
        }
    }
}