using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IScheduleTaskData : IAsyncRepository<ScheduleTask>
    {
        /// <summary>
        /// Hàm lấy các công việc theo lịch của hệ thống theo nhiều tham số.
        /// </summary>
        /// <param name="skip">Number row skip</param>
        /// <param name="take">Number row take</param>
        /// <param name="name">Tên công việc</param>
        /// <param name="type">Loại công việc</param>
        /// <param name="enabled">Trạng thái bật tắt</param>
        /// <param name="stopOnError">Dừng lại khi lỗi</param>
        /// <returns>IQueryable ScheduleTask</returns>
        Task<IDataSourceResult<ScheduleTask>> GetScheduleTasksAsync(int skip, int take, string name = "", ScheduleType? type = null, bool? enabled = null, bool? stopOnError = null);
    }
}