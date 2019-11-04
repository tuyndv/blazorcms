using Pl.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface ICmsMenuService
    {
        /// <summary>
        /// Lấy danh sách menu theo quyền người dùng
        /// </summary>
        /// <param name="roles">Danh sách quyền</param>
        /// <returns>List Menu</returns>
        Task<IReadOnlyList<CmsMenu>> GetMenusByRolesAsync(List<string> roles);

        /// <summary>
        /// Lấy danh sách tree menu dựa vào vị trí
        /// </summary>
        /// <param name="position">Vị trí của menu</param>
        Task<IEnumerable<TreeItem<CmsMenu>>> CacheGetByPositionAsync(string position);

        /// <summary>
        /// Get all cms menu in system
        /// </summary>
        /// <returns>Task IReadOnlyList CmsMenu</returns>
        Task<IReadOnlyList<CmsMenu>> CacheGetAllAsync();

        /// <summary>
        /// Lấy tên cây thư mục
        /// </summary>
        /// <param name="childId">Id menu con</param>
        /// <param name="separater">Dấu phân cách</param>
        /// <returns>Task string</returns>
        Task<string> GetTreeNameAsync(string childId, string separater = " > ");
    }
}