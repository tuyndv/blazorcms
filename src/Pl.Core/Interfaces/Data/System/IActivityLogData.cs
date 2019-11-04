using Pl.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IActivityLogData : IAsyncRepository<ActivityLog>
    {
        /// <summary>
        /// Lấy danh sách nhất ký hoạt động của người dùng cho trang cms dùng kendoGrid
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="ipAddress">Địa chỉ ip</param>
        /// <param name="message">Nội dung log</param>
        /// <param name="userId">Id người tạo log</param>
        /// <param name="activityType">Id loại hành động</param>
        /// <param name="startCreatedTime">Ngày tạo bắt đầu</param>
        /// <param name="endCreatedTime">Ngày tạo kết thúc</param>
        /// <returns></returns>
        Task<IDataSourceResult<ActivityLog>> GetActivityLogsAsync(int skip, int take, string ipAddress = "", string message = "", string userId = null, ActivityTypeEnum? activityType = null, DateTime? startCreatedTime = null, DateTime? endCreatedTime = null);

        /// <summary>
        /// Hàm xóa hết hàng động theo người dùng
        /// Chạy không đồ bộ với luồng hệ thống
        /// </summary>
        /// <param name="userId">id người dùng</param>
        /// <returns>bool</returns>
        Task DeleteByUserIdAsync(string userId);

        /// <summary>
        /// Lấy dữ liệu thống kê
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu</param>
        /// <param name="endDate">Ngày kết thúc</param>
        /// <param name="userId">Id người dùng</param>
        /// <returns>Dictionary string int</returns>
        Task<Dictionary<string, int>> GetStatisticalByDayAsync(DateTime startDate, DateTime endDate, string userId = "");

    }
}