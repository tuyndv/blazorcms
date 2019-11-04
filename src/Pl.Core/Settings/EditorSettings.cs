namespace Pl.Core.Settings
{
    public class EditorSettings
    {
        /// <summary>
        /// Đường dẫn domain url ảnh trong editer
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Đường dẫn thư mục upload ảnh trong editer
        /// </summary>
        public string BaseDir { get; set; }

        /// <summary>
        /// Danh sách đuôi file hợp lệ trong editer  phân cách nhau bằng 1 dấu ,
        /// </summary>
        public string FileAllowedExtensions { get; set; }

        /// <summary>
        /// Danh sách đuôi file ảnh hợp lệ trọng editor  phân cách nhau bằng 1 dấu ,
        /// </summary>
        public string ImageAllowedExtensions { get; set; }

        /// <summary>
        /// Danh sách duôi file flash trong editor  phân cách nhau bằng 1 dấu ,
        /// </summary>
        public string FlashAllowedExtensions { get; set; }

        /// <summary>
        /// Cho phép tạo ảnh thumb hay không
        /// </summary>
        public bool ThumbnailsEnable { get; set; }

        /// <summary>
        /// Kích thước tối đã của file upload lên
        /// </summary>
        public int FileMaxSize { get; set; }

        /// <summary>
        /// Kích thước tối đa của ảnh upload lên
        /// </summary>
        public int ImageMaxSize { get; set; }

        /// <summary>
        /// Kích thước tôi đa của file flash upload lên
        /// </summary>
        public int FlashMaxSize { get; set; }

        /// <summary>
        /// Kích thước chiểu rộng ảnh tối đa
        /// </summary>
        public int ImageWidthMax { get; set; }

        /// <summary>
        /// Kích thước chiểu cao ảnh tối đa
        /// </summary>
        public int ImageHeightMax { get; set; }
    }
}