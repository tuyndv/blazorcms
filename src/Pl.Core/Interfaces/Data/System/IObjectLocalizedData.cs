using Pl.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IObjectLocalizedData : IAsyncRepository<ObjectLocalized>
    {
        /// <summary>
        /// Hàm lấy giá trị của localizedValue
        /// </summary>
        /// <param name="languageCulture">Mã ngôn ngữ</param>
        /// <param name="objectId">Id đối tượng</param>
        /// <param name="objectType">Id loại đối tượng</param>
        /// <param name="propertyName">Tên thuộc tính</param>
        /// <param name="stringIfNull">Trường hợp chưa có sẽ trả về giá trị này.</param>
        /// <returns>string</returns>
        Task<string> GetLocalizedValueAsync(string languageCulture, string objectId, ObjectTypeEnum objectType, string propertyName, string stringIfNull = null);

        /// <summary>
        /// Lưu lại thông tin ngôn ngữ
        /// </summary>
        /// <param name="languageCulture">Mã ngôn ngữ</param>
        /// <param name="objectId">Id loại đối tượng</param>
        /// <param name="objectType">Id đối tượng</param>
        /// <param name="propertyName">Tên thuộc tính của đối tượng</param>
        /// <param name="value">Giá trị ngôn ngữ</param>
        /// <returns>bool</returns>
        Task<bool> SaveLocalizedAsync(string languageCulture, string objectId, ObjectTypeEnum objectType, string propertyName, string value);

        /// <summary>
        /// Xóa giá trị ngôn ngữ
        /// </summary>
        /// <param name="languageCulture">Culture of language to delete</param>
        /// <param name="objectId">Id đối tượng</param>
        /// <param name="objectTypeId">Id loại đối tượng</param>
        /// <param name="propertyName">Tên thuộc tính</param>
        /// <returns>bool</returns>
        Task<bool> DeleteLocalizedAsync(string languageCulture = null, string objectId = null, ObjectTypeEnum? objectTypeId = null, string propertyName = "");

        /// <summary>
        /// Hàm này sẽ set giá trị Ngôn ngữ vào object
        /// </summary>
        /// <typeparam name="T">Kiểu của object cần set</typeparam>
        /// <param name="entity">Object cần sét</param>
        /// <param name="languageCulture">Mã ngôn ngữ</param>
        /// <param name="objectType">Loại đối tượng cần binding ngôn ngữ</param>
        /// <returns>Object đã được thay đổi localized</returns>
        Task<T> GetLocalizedStringAsync<T>(T entity, string languageCulture, ObjectTypeEnum objectType) where T : IBaseEntity;

        /// <summary>
        /// set language localized to list object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="entitys">List object</param>
        /// <param name="languageCulture">Language culture need set</param>
        /// <param name="objectType">Type of object enum</param>
        /// <returns>Object đã được thay đổi localized</returns>
        Task<IEnumerable<T>> GetLocalizedStringAsync<T>(IEnumerable<T> entitys, string languageCulture, ObjectTypeEnum objectType) where T : IBaseEntity;

        /// <summary>
        /// Hàm này sẽ set giá trị Ngôn ngữ object vào db.
        /// </summary>
        /// <typeparam name="T">Loại đối tượng</typeparam>
        /// <param name="entity">Đối tượng</param>
        /// <param name="objectId">Id của đối tuộng</param>
        /// <param name="languageCulture">Mã ngôn ngữ</param>
        /// <param name="objectType">Id loại đối tượng quản lý trong hệ thống</param>
        Task<bool> SetLocalizedStringAsync<T>(T entity, string objectId, string languageCulture, ObjectTypeEnum objectType);

        /// <summary>
        /// Ghi list model localized vào db
        /// </summary>
        /// <typeparam name="T">Class model</typeparam>
        /// <param name="listLocalizedData">List model localized</param>
        /// <param name="objectId">Id của ef Entity</param>
        /// <param name="objectType">Id loại đối tượng</param>
        /// <returns>bool</returns>
        Task SetLocalizedModelLocalAsync<T>(IEnumerable<T> listLocalizedData, string objectId, ObjectTypeEnum objectType);

        /// <summary>
        /// Lấy localized của object
        /// </summary>
        /// <typeparam name="T">Kiểu của model</typeparam>
        /// <param name="objectId">ID của ef entity</param>
        /// <param name="objectType">Id object Type</param>
        /// <returns>List localized</returns>
        Task<List<T>> GetLocalizedModelLocalAsync<T>(string objectId = null, ObjectTypeEnum? objectType = null) where T : IBaseEntity;
    }
}