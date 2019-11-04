namespace Pl.Core.Entities
{
    public class LanguageResource : BaseEntity
    {
        /// <summary>
        /// Culture name of resource
        /// ex. vi-VN
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Key resource
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Type of resource
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Giá trị ngôn ngữ
        /// </summary>
        public string Value { get; set; }
    }
}