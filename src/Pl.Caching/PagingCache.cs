namespace Pl.Caching
{
    /// <summary>
    /// hỗ trợ phân trang trong cache
    /// </summary>
    /// <typeparam name="TItem">Loại danh sách đối tượng</typeparam>
    public class PagingCache<TItem>
    {
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int RowsCount { get; set; }

        /// <summary>
        /// Danh sách đối tượng
        /// </summary>
        public TItem Data { get; set; }
    }
}