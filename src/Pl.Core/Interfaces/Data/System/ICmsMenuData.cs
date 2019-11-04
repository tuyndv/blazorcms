using Pl.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface ICmsMenuData : IAsyncRepository<CmsMenu>
    {
        /// <summary>
        /// Lấy danh sách menu theo nhiều tiêu chí, xắp xếp theo thứ tự
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="position">Vị trí</param>
        /// <param name="active">Trạng thái hiển thị</param>
        /// <param name="name">Tên menu</param>
        /// <param name="parentId">Id chuyên mục cha</param>
        /// <returns>Task IDataSourceResult CmsMenu</returns>
        Task<IDataSourceResult<CmsMenu>> GetMenusAsync(int skip, int take, string position = "", bool? active = null, string name = null, string parentId = null);

        /// <summary>
        /// Lấy danh sách menu dùng cho các select dropdowlist
        /// </summary>
        /// <param name="position">Vi trí</param>
        /// <param name="active">Trạng thái active</param>
        /// <param name="parentId">Id menu cha</param>
        /// <param name="excludeId">Id bỏ qua</param>
        /// <returns>Task IEnumerable TreeItem CmsMenu</returns>
        Task<IEnumerable<TreeItem<CmsMenu>>> GetMenusAsync(string position = "", bool? active = null, string parentId = null, string excludeId = null);

        /// <summary>
        /// Xuất menu ra file json
        /// </summary>
        /// <returns>Chuỗi nội dung file json</returns>
        Task<string> ExportToJsonAsync();

        /// <summary>
        /// Nhập nội dung file meu từ json
        /// </summary>
        /// <param name="json">Nội dung file json</param>
        Task<bool> ImportFromJsonAsync(string json);
    }
}