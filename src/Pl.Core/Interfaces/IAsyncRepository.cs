using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    /// <summary>
    /// Định nhĩa các hàm cơ bản để làm việc đọc ghi dữ liệu cho một đối tượng trong db
    /// </summary>
    /// <typeparam name="T">Loại đối tượng</typeparam>
    public interface IAsyncRepository<T> where T : class, IBaseEntity
    {
        #region Check methods

        /// <summary>
        /// Check existed item by perdicate
        /// </summary>
        /// <param name="predicate">the any predicate</param>
        /// <returns>bool</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        #endregion

        #region Get methods

        /// <summary>
        /// Find a item by primary key
        /// </summary>
        /// <param name="primaryKey">Key value</param>
        /// <returns>T</returns>
        Task<T> FindAsync(string primaryKey);

        /// <summary>
        /// Find a item by primary key with no tracking
        /// </summary>
        /// <param name="primaryKey">Key value</param>
        /// <returns>T</returns>
        Task<T> FindNoTrackingAsync(string primaryKeys);

        /// <summary>
        /// Lấy một object bất đồng bộ theo điều kiện
        /// </summary>
        /// <param name="predicate">Điều kiện cần lấy</param>
        /// <returns>T</returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Lấy một object bất đồng bộ theo điều kiện, và không theo dõi đối tượng sau khi lấy ra
        /// </summary>
        /// <param name="predicate">Điều kiện cần lấy</param>
        /// <returns>T</returns>
        Task<T> FindNoTrackingAsync(Expression<Func<T, bool>> predicate);

        #endregion

        #region Counter methods

        /// <summary>
        /// lấy tổng số bản ghi dựa vào biểu thức điều kiện
        /// </summary>
        /// <param name="predicate">Biểu thức điều kiện, để null nếu muốn lấy tất cả</param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// lấy tổng số bản ghi dựa vào specification
        /// </summary>
        /// <param name="specification">Thông tin điều kiện</param>
        /// <returns></returns>
        Task<int> CountAsync(ISpecification<T> specification);

        #endregion

        #region Listing methods

        /// <summary>
        /// Lấy toàn bộ tập hợp các đối tượng
        /// </summary>
        /// <returns>IQueryable T</returns>
        Task<IReadOnlyList<T>> FindAllAsync();

        /// <summary>
        /// Lấy toàn bộ tập hợp các đối tượng và không theo dõi thay đổi
        /// </summary>
        /// <returns>IQueryable T</returns>
        Task<IReadOnlyList<T>> FindAllNoTrackingAsync();

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="predicate">Điều kiện lấy</param>
        /// <returns>IQueryable T</returns>
        Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="predicate">Điều kiện lấy</param>
        /// <returns>IQueryable T</returns>
        Task<IReadOnlyList<T>> FindAllNoTrackingAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="specification">Thông tin tiêu chí lấy dữ liệu</param>
        /// <returns>IQueryable T</returns>
        Task<IReadOnlyList<T>> FindAllAsync(ISpecification<T> specification);

        /// <summary>
        /// Lấy danh sách nhưng không theo dõi các đối tượng lấy ra
        /// </summary>
        /// <param name="specification">Thông tin tiêu chí lấy dữ liệu</param>
        /// <returns>IQueryable T</returns>
        Task<IReadOnlyList<T>> FindAllNoTrackingAsync(ISpecification<T> specification);

        /// <summary>
        /// Lấy một DataSourceResult, các đối tượng lấy ra không được theo dõi
        /// </summary>
        /// <param name="specification">Thông tin tiêu chí lấy dữ liệu</param>
        /// <returns>IDataSourceResult</returns>
        Task<IDataSourceResult<T>> ToDataSourceResultAsync(ISpecification<T> specification);

        #endregion Listing Methods

        #region Insert methods

        /// <summary>
        /// Thêm mới một T bất đồng bộ
        /// </summary>
        /// <param name="entity">đối tượng cần thêm mới</param>
        /// <returns>bool</returns>
        Task<bool> InsertAsync(T entity);

        /// <summary>
        /// Thêm mới một tập hợp các đối tượng bất đồng bộ
        /// </summary>
        /// <param name="entities">Tập hợp các đối tượng</param>
        /// <returns>bool</returns>
        Task<bool> InsertAsync(IEnumerable<T> entities);

        #endregion Insert methods

        #region Update methods

        /// <summary>
        /// Cập nhập một đối tượng
        /// </summary>
        /// <param name="T">Đối tượng</param>
        /// <returns>bool</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Cập nhập một tập hợp các đối tượng bất đồng bộ
        /// </summary>
        /// <param name="entities">Tập hợp các đối tượng cần thêm mới</param>
        Task<bool> UpdateAsync(IEnumerable<T> entities);

        #endregion Update methods

        #region Delete methods

        /// <summary>
        /// Xóa toàn bộ dự liệu trong bản và restart lại index. Bất đồng bộ
        /// </summary>
        /// <returns>bool</returns>
        Task<bool> TruncateAsync();

        /// <summary>
        /// Xóa một tập hợp dữ liệu, đảm bảo không còn giàn buộc dữ liệu khi gọi hàm này bất đồng bộ
        /// </summary>
        /// <param name="entities">Tập hợp dữ liệu</param>
        /// <returns>bool</returns>
        Task<bool> DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// Xóa một đối tượng bất đồng bộ
        /// </summary>
        /// <param name="entity">Đối tượng thật cần xóa</param>
        /// <returns>bool</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Xóa một danh sách đối tượng theo danh sách khóa chính
        /// </summary>
        /// <param name="primaryKeys">danh sách khóa chính</param>
        /// <returns>bool</returns>
        Task<bool> DeleteByKeysAsync(List<string> primaryKeys);

        /// <summary>
        /// Xóa một đối tượng theo khóa chính
        /// </summary>
        /// <param name="primaryKey">khóa chính</param>
        /// <returns>bool</returns>
        Task<bool> DeleteByKeyAsync(string primaryKey);

        #endregion Delete methods

        #region QueryString method

        /// <summary>
        /// Hàm chạy một sql command bất đồng bộ
        /// </summary>
        /// <param name="queryCommand">cấu query cần chạy</param>
        /// <param name="isolationLevel">Mức độ khóa dữ liệu khi thực hiện execute</param>
        /// <returns>bool</returns>
        Task<bool> ExecuteSqlCommandAsync(string queryCommand, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Hàm chạy một sql command bất đồng bộ
        /// </summary>
        /// <param name="queryCommand">cấu query cần chạy</param>
        /// <param name="isolationLevel">Mức độ khóa dữ liệu khi thực hiện execute</param>
        /// <param name="parameters">parameters</param>
        /// <returns>bool</returns>
        Task<bool> ExecuteSqlCommandAsync(string queryCommand, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, params object[] parameters);

        /// <summary>
        /// Hàm chạy một sql command bất đồng bộ
        /// </summary>
        /// <param name="queryCommand">cấu query cần chạy</param>
        /// <param name="isolationLevel">Mức độ khóa dữ liệu khi thực hiện execute</param>
        /// <returns>IQueryable T</returns>
        Task<IReadOnlyList<T>> SqlQueryAsync(string queryCommand, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Hàm chạy một sql command
        /// </summary>
        /// <param name="queryCommand">cấu query cần chạy</param>
        /// <param name="isolationLevel">Mức độ khóa dữ liệu khi thực hiện execute</param>
        /// <param name="parameters">Danh sách các sql parameters</param>
        /// <returns>List T</returns>
        Task<IReadOnlyList<T>> SqlQueryAsync(string queryCommand, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, params object[] parameters);

        #endregion QueryString Method
    }
}