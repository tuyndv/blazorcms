using Pl.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IUserData : IAsyncRepository<User>
    {
        /// <summary>
        /// Lấy danh sách người dùng không traking db context
        /// </summary>
        /// <param name="skip">Skip row value</param>
        /// <param name="take">Take row value</param>
        /// <param name="groupId">Nhóm người dùng</param>
        /// <param name="email">Email người dùng</param>
        /// <param name="userName">Tài khoản người dùng</param>
        /// <param name="displayName">Tên hiển thị</param>
        /// <param name="active">Trạng thái active</param>
        /// <param name="gender">Giới tính</param>
        /// <param name="languageCulture">Mã ngôn ngữ</param>
        /// <param name="currencyCulture">Mã tiền tệ</param>
        /// <param name="fromBirthDay">Sinh từ ngày</param>
        /// <param name="toBirthDay">Sinh đến ngày</param>
        /// <param name="fromLastLoginTime">Lần đăng nhập cuối từ</param>
        /// <param name="toLastLoginTime">Lần đăng nhập cuối đến</param>
        /// <param name="fromCreatedTime">Tạo từ ngày</param>
        /// <param name="toCreatedTime">Tạo đến ngày</param>
        Task<IDataSourceResult<User>> GetUsersAsync(
            int skip,
            int take,
            string groupId = null,
            string email = null,
            string userName = null,
            string displayName = null,
            bool? active = null,
            Gender? gender = null,
            string languageCulture = null,
            string currencyCulture = null,
            DateTime? fromBirthDay = null,
            DateTime? toBirthDay = null,
            DateTime? fromLastLoginTime = null,
            DateTime? toLastLoginTime = null,
            DateTime? fromCreatedTime = null,
            DateTime? toCreatedTime = null);

        /// <summary>
        /// Lấy thông tin đại diện cho người dùng trọng hệ thống
        /// </summary>
        /// <param name="userId">Id người dùng</param>
        /// <param name="isEmail">Lấy email hoặc tên hiển thị</param>
        /// <returns>Mặc định trả về email</returns>
        Task<string> GetInfoByUserIdAsync(string userId, bool isEmail = false);
    }
}