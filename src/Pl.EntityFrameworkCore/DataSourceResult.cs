using Pl.Core.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace Pl.EntityFrameworkCore
{
    /// <summary>
    /// Data source result support kendo ui
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataSourceResult<T> : IDataSourceResult<T>
    {
        /// <summary>
        /// Danh sách dữ liệu
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Dữ liệu mở rộng
        /// </summary>
        public object ExtraData { get; set; } = null;

        /// <summary>
        /// Danh sách nhóm
        /// </summary>
        public IEnumerable Group { get; set; } = null;

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// Đối tượng tập hợp
        /// </summary>
        public object Aggregates { get; set; } = null;

        /// <summary>
        /// Lỗi nếu có
        /// </summary>
        public object Errors { get; set; } = null;
    }
}