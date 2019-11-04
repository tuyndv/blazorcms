namespace Pl.Core.Entities
{
    /// <summary>
    /// Bảng mapping media sang các đối tượng khác
    /// </summary>
    public class FileResourceMapping : BaseEntity
    {
        /// <summary>
        /// Id file
        /// </summary>
        public string FileResourceId { get; set; }

        /// <summary>
        /// Id của đối tượng
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Type of object
        /// </summary>
        public ObjectTypeEnum ObjectType { get; set; }

        /// <summary>
        /// Trạng thái đăng
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Tiêu đều của mapping
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mô tả cho mapping có thể la html
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// The link of mapping
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Số thứ tự
        /// </summary>
        public int DisplayOrder { get; set; } = 1;
    }
}