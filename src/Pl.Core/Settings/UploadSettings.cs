using System.Globalization;

namespace Pl.Core.Settings
{
    /// <summary>
    /// Quy định tất cả các vấn đề về upload ngoại trừ bộ editor
    /// Tất cả các domain đều kết thúc bằng dấu /
    /// </summary>
    public class UploadSettings
    {
        #region Image

        /// <summary>
        /// Domain ảnh
        /// </summary>
        public string DomainImgPath { get; set; }

        /// <summary>
        /// Đường dẫn upload của ảnh
        /// </summary>
        public string UploadImgPath { get; set; }

        /// <summary>
        /// Danh sách đuôi ảnh hợp lệ  phân cách nhau bằng 1 dấu ,
        /// </summary>
        public string ImageAllowedExtensions { get; set; }

        /// <summary>
        /// Kích thước tối đa của ảnh upload lên
        /// </summary>
        public int ImageMaxSize { get; set; }

        /// <summary>
        /// Những kích thước ảnh được phép resize
        /// </summary>
        public string ImgResizeList { get; set; }

        /// <summary>
        /// Tỉ lệ ảnh mặc định của hệ thống
        /// dạng 700x300 đơn vị tính px
        /// </summary>
        public string DefaultImageFrameRate { get; set; }

        /// <summary>
        /// Lấy trạng thái đã paser của tỉ lệ ảnh mặc định
        /// </summary>
        public (int W, int H) ImageFrameRateParser
        {
            get
            {
                if (!string.IsNullOrEmpty(DefaultImageFrameRate))
                {
                    string[] frameParser = DefaultImageFrameRate.Split("x");
                    return (int.Parse(frameParser[0], CultureInfo.CurrentCulture), int.Parse(frameParser[1], CultureInfo.CurrentCulture));
                }
                return (16, 9);
            }
        }

        #endregion Image

        #region Video

        /// <summary>
        /// Domain video
        /// </summary>
        public string DomainVideoPath { get; set; }

        /// <summary>
        /// Đường dẫn upload của video
        /// </summary>
        public string UploadVideoPath { get; set; }

        /// <summary>
        /// Danh sách đuôi vido hợp lệ phân cách nhau bằng 1 dấu ,
        /// </summary>
        public string VideoAllowedExtensions { get; set; }

        /// <summary>
        /// Kích thước tối đa của video upload lên
        /// </summary>
        public int VideoMaxSize { get; set; }

        #endregion Video

        #region File

        /// <summary>
        /// Domain của file upload
        /// </summary>
        public string DomainFilePath { get; set; }

        /// <summary>
        /// Đường dẫn upload của file
        /// </summary>
        public string UploadFilePath { get; set; }

        /// <summary>
        /// Danh sách đuôi file hợp lệ phân cách nhau bằng 1 dấu ,
        /// </summary>
        public string FileAllowedExtensions { get; set; }

        /// <summary>
        /// Kích thước tối đa của file upload lên
        /// </summary>
        public int FileMaxSize { get; set; }

        #endregion File

        #region Audio

        /// <summary>
        /// Domain của audio upload
        /// </summary>
        public string DomainAudioPath { get; set; }

        /// <summary>
        /// Đường dẫn upload của audio
        /// </summary>
        public string UploadAudioPath { get; set; }

        /// <summary>
        /// Danh sách đuôi audio hợp lệ phân cách nhau bằng 1 dấu ,
        /// </summary>
        public string AudioAllowedExtensions { get; set; }

        /// <summary>
        /// Kích thước tối đa của audio upload lên
        /// </summary>
        public int AudioMaxSize { get; set; }

        #endregion Audio
    }
}