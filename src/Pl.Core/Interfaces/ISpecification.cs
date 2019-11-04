using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pl.Core.Interfaces
{
    /// <summary>
    /// Lớp định nghĩa các quy tắc để lấy dữ liệu từ một nguồn
    /// </summary>
    /// <typeparam name="T">Loại dữ liệu cần lấy</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Các tiêu chí cần lấy
        /// </summary>
        Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Lấy thêm vào, bao gồm thêm vào đối tượng cần lấy
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Lấy thêm, bao gồm thêm đối tượng bằng cây string
        /// </summary>
        List<string> IncludeStrings { get; }

        /// <summary>
        /// Xắp xếp danh sách lấy ra
        /// </summary>
        Expression<Func<T, object>> OrderBy { get; }

        /// <summary>
        /// Xắp xếp danh sách lấy ra
        /// </summary>
        Expression<Func<T, object>> OrderByDescending { get; }

        /// <summary>
        /// Chỉ dõ nếu muốn lấy 1 phần object
        /// </summary>
        Expression<Func<T, T>> Selector { get; }

        /// <summary>
        /// Nhóm đối tượng theo biểu thức
        /// </summary>
        Expression<Func<T, object>> GroupBy { get; }

        /// <summary>
        /// Số bản ghi cần lấy
        /// </summary>
        int Take { get; }

        /// <summary>
        /// Số bản ghi bỏ qua
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Trạng thái có phân trang hay không
        /// </summary>
        bool IsPagingEnabled { get; }
    }
}