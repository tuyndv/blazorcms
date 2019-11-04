namespace Pl.Core.Entities
{
    public class SystemValue : BaseEntity
    {
        /// <summary>
        /// Khóa giá trị
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Giá trị
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
    }
}