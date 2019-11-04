namespace Pl.Core.Entities
{
    public class Language : BaseEntity
    {
        /// <summary>
        /// Tên ngôn ngữ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã ngôn ngữ dang en-US, vi-VN ...
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Tên cờ ngôn ngữ
        /// </summary>
        public string FlagImage { get; set; }

        /// <summary>
        /// Trạng thái đăng
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; } = 1;

        /// <summary>
        /// Là ngôn ngữ mặc định trong hệ thống
        /// </summary>
        public bool DisplayDefault { get; set; }

        /// <summary>
        /// Lấy mã ngôn ngữ dạng 2 ký tự identity
        /// </summary>
        public virtual string TwoLanguageCode
        {
            get
            {
                if (!string.IsNullOrEmpty(Culture))
                {
                    if (Culture.Length > 2)
                    {
                        return Culture.Substring(0, 2);
                    }
                }

                return Culture;
            }
        }
    }
}