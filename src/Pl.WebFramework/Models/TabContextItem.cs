namespace Pl.WebFramework.Models
{
    public class TabContextItem
    {
        /// <summary>
        /// Tiêu đề tab
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// Nội dung tab
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// Tab mặc định
        /// </summary>
        public bool IsDefault { set; get; }
    }
}