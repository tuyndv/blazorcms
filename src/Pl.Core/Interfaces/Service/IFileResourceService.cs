using Pl.Core.Entities;

namespace Pl.Core.Interfaces
{
    public interface IFileResourceService
    {
        /// <summary>
        /// Hàm lấy loại resource dựa vào tên của resource
        /// </summary>
        /// <param name="resourcePath">Tên của resource</param>
        /// <returns>
        /// default ResourceType.None
        /// </returns>
        ResourceType GetMediaType(string resourcePath);

        /// <summary>
        /// Hàm tạo ảnh thumb khi upload ảnh
        /// </summary>
        /// <param name="thumbPath">Đường dẫn đến thư mục lưu ảnh thumb</param>
        /// <param name="imagePath">Đường dẫn đền ảnh gốc</param>
        /// <param name="resizeList">Danh sách kích thước cần resize dạng 100*200,300</param>
        void CreateThumbImage(string thumbPath, string imagePath, string resizeList);

        /// <summary>
        /// Hàm lấy đường dẫn file dựa vào loại file và tên file
        /// </summary>
        /// <param name="fileType">Loại file</param>
        /// <param name="fileName">Tên file</param>
        /// <returns>string</returns>
        string GetFilePath(ResourceType fileType, string fileName);
    }
}