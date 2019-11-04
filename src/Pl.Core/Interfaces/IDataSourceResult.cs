using System.Collections;
using System.Collections.Generic;

namespace Pl.Core.Interfaces
{
    public interface IDataSourceResult<T>
    {
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        int Total { get; set; }

        /// <summary>
        /// Danh sách dữ liệu
        /// </summary>
        IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Danh sách nhóm
        /// </summary>
        IEnumerable Group { get; set; }

        /// <summary>
        /// Dữ liệu mở rộng
        /// </summary>
        object ExtraData { get; set; }

        /// <summary>
        /// Đối tượng tập hợp
        /// </summary>
        object Aggregates { get; set; }

        /// <summary>
        /// Lỗi nếu có
        /// </summary>
        object Errors { get; set; }
    }
}