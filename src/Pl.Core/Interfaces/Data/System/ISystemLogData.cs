using Microsoft.Extensions.Logging;
using Pl.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface ISystemLogData : IAsyncRepository<SystemLog>
    {
        /// <summary>
        /// Hàm lấy Error log theo nhiều tham số
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="logLevelId">Mức độ lỗi</param>
        /// <param name="shortMessage">Nội nỗi ngắn</param>
        /// <param name="ipAdress">Id gây lỗi</param>
        /// <param name="startDate">Từ ngày</param>
        /// <param name="endDate">Đến ngày</param>
        /// <returns>IDataSourceResult ErrorLog</returns>
        Task<IDataSourceResult<SystemLog>> GetErrorLogsAsync(
            int skip,
            int take,
            LogLevel? logLevelId = null,
            string shortMessage = "",
            string ipAdress = "",
            DateTime? startDate = null,
            DateTime? endDate = null);
    }
}