using Pl.Core.Exceptions;
using System;

namespace Pl.Core.Specifications
{
    /// <summary>
    /// Thông tin phân trang trong hệ thống.
    /// </summary>
    public class Paging
    {
        #region Private properties

        private int _rowsCount;

        #endregion Private properties

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="currentPage">Trang hiện tại</param>
        public Paging(int pageSize = 10, int currentPage = 1)
        {
            GuardClausesParameter.OutOfRange(pageSize, nameof(pageSize), 1, int.MaxValue);

            PageSize = pageSize;
            CurrentPage = currentPage;
            _rowsCount = 0;
        }

        #region Properties And Constructor

        /// <summary>
        /// Dòng dữ liệu bắt đầu cần lấy
        /// </summary>
        public int StartRowIndex => PageSize * (CurrentPage - 1);

        /// <summary>
        /// Số bản ghi trên một trang
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int RowsCount
        {
            get => _rowsCount;
            set
            {
                GuardClausesParameter.OutOfRange(value, nameof(value), 0, int.MaxValue);
                _rowsCount = value;
            }
        }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PagesCount => (int)Math.Ceiling(_rowsCount / (double)PageSize);

        /// <summary>
        /// Còn có thể đến trang tiếp theo
        /// </summary>
        public bool HasNextPage => CurrentPage < PagesCount;

        /// <summary>
        /// Còn có thể quay chở lại
        /// </summary>
        public bool HasBackPage => CurrentPage > 0;

        #endregion Properties And Constructor

        #region Method

        /// <summary>
        /// Hàm tạo cache key cho thông tin phân trang
        /// </summary>
        /// <returns>string</returns>
        public virtual string ToCacheKey()
        {
            return $"_paif_{CurrentPage}_{PageSize}";
        }

        #endregion Method
    }
}