using Pl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pl.Core.Specifications
{
    /// <summary>
    /// Tổng hợp quy tắc để lấy dữ liệu
    /// </summary>
    /// <typeparam name="T">Loại đối tượng cần lấy</typeparam>
    public class BaseSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Khởi tạo ít nhất phải có điều kiện, tiêu chí lấy
        /// </summary>
        /// <param name="criteria"></param>
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <summary>
        /// Các tiêu chí cần lấy
        /// </summary>
        public Expression<Func<T, bool>> Criteria { get; }

        /// <summary>
        /// Lấy thêm vào, bao gồm thêm vào đối tượng cần lấy
        /// </summary>
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// Lấy thêm, bao gồm thêm đối tượng bằng cây string
        /// </summary>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <summary>
        /// Chỉ dõ nếu muốn lấy 1 phần object
        /// </summary>
        public Expression<Func<T, T>> Selector { get; private set; } = null;

        /// <summary>
        /// Xắp xếp danh sách lấy ra
        /// </summary>
        public Expression<Func<T, object>> OrderBy { get; private set; }

        /// <summary>
        /// Xắp xếp danh sách lấy ra
        /// </summary>
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        /// <summary>
        /// Nhóm đối tượng theo biểu thức
        /// </summary>
        public Expression<Func<T, object>> GroupBy { get; private set; }

        /// <summary>
        /// Số bản ghi cần lấy
        /// </summary>
        public int Take { get; private set; }

        /// <summary>
        /// Số bản ghi bỏ qua
        /// </summary>
        public int Skip { get; private set; }

        /// <summary>
        /// Trạng thái có phân trang hay không
        /// </summary>
        public bool IsPagingEnabled { get; private set; } = false;

        /// <summary>
        /// Thêm các đối tượng vào dữ liệu lấy ra
        /// </summary>
        /// <param name="includeExpression">Đối tượng include</param>
        public virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Thêm dữ liệu lấy ra dựa vào tree string
        /// </summary>
        /// <param name="includeString">Chuỗi string cần include</param>
        public virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        /// <summary>
        /// Thêm xăp xếp
        /// </summary>
        /// <param name="orderByExpression">Thông tin xắp xếp</param>
        public virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Thêm xắp xếp
        /// </summary>
        /// <param name="orderByDescendingExpression">Thông tin xắp xếp</param>
        public virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        /// <summary>
        /// Thêm chỉ định các phần của một object cần lấy
        /// </summary>
        /// <param name="selecter">Định nghĩa các phần cần lấy</param>
        public virtual void ApplySelector(Expression<Func<T, T>> selecter)
        {
            Selector = selecter;
        }

        /// <summary>
        ///  Áp dụng nhóp đối tượng theo biểu thức
        /// </summary>
        /// <param name="groupByExpression">Biểu thúc nhóm</param>
        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }

        /// <summary>
        /// Áp dụng phân trang cho điều kiện
        /// </summary>
        /// <param name="skip">Số bản ghi cần bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        public virtual void ApplyPaging(int skip, int take)
        {
            if (0 == skip && 0 == take)
            {
                throw new Exception("Take and skip out of range.");
            }

            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}