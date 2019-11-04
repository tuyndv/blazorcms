namespace Pl.Core.Entities
{
    public enum ResourceType
    {
        /// <summary>
        /// Default file type
        /// </summary>
        None,

        /// <summary>
        /// file images
        /// </summary>
        Image,

        /// <summary>
        /// Video file
        /// </summary>
        Movie,

        /// <summary>
        /// Audio file
        /// </summary>
        Audio,

        /// <summary>
        /// Avata file
        /// </summary>
        Avatar
    }

    public class FileResource : BaseEntity
    {
        /// <summary>
        /// path of file ex. 2017/03/27/filename.jpg
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Mô tả tệp tin
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Loại media xẽ được xử lý khi ghi lại media
        /// </summary>
        public ResourceType Type { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Kích thước file dưới dang byte
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Lấy đuôi mở rộng của file
        /// </summary>
        public string Extension => CoreUtility.GetFileExtension(Path, false);

        /// <summary>
        /// Chỉ lấy tên file
        /// </summary>
        public string Name => CoreUtility.GetFileName(Path);
    }
}