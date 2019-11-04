namespace Pl.Core.Entities
{
    public class ObjectLocalized : BaseEntity
    {
        /// <summary>
        /// Id ngôn ngữ
        /// </summary>
        public string LanguageCulture { get; set; }

        /// <summary>
        /// Id đối tượng
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Id loại đối tượng
        /// </summary>
        public ObjectTypeEnum ObjectType { get; set; }

        /// <summary>
        /// Tên của thuộc tính
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Giá trị ngôn ngữ
        /// </summary>
        public string LocalizedValue { get; set; }
    }
}