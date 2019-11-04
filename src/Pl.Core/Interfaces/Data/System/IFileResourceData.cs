using Pl.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IFileResourceData : IAsyncRepository<FileResource>
    {

        /// <summary>
        /// Lấy danh sách thư viên tệp tin theo nhiều tham số
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="description">Mô tả cho file</param>
        /// <param name="type">Loại resource</param>
        /// <param name="userId">Người tạo tệp tin</param>
        /// <param name="startDate">Bắt đầu ngày tạo</param>
        /// <param name="endDate">Kết thúc ngày tạo</param>
        /// <returns>IDataSourceResult Media</returns>
        Task<IDataSourceResult<FileResource>> GetFileResourcesAsync(
            int skip,
            int take,
            string description = "",
            ResourceType? type = null,
            string userId = null,
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Delete FileResource file
        /// </summary>
        /// <param name="fileResource">FileResource to delete</param>
        Task DeleteFileAsync(FileResource fileResource);

        /// <summary>
        /// Hàm xóa ảnh được upload lên
        /// </summary>
        /// <param name="imageFileName">Tên file ảnh cần xóa</param>
        Task DeleteImageFileAsync(string imageFileName);

        /// <summary>
        /// Hàm xóa video được upload lên
        /// </summary>
        /// <param name="fileName">Tên file cần xóa</param>
        Task DeleteFileAsync(string fileName);

    }
}