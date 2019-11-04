using System;

namespace Pl.Core.Entities
{
    public enum ScheduleType
    {
        /// <summary>
        /// key get schedule info to process try send email if failed send in queue
        /// </summary>
        TrySendFailedEmail,

        /// <summary>
        /// Key get schedule auto create cache and refresh cache data
        /// </summary>
        CreateAndRefreshCache,

        /// <summary>
        /// Key get schedule recurring delete log
        /// </summary>
        RecurringDeleteLog
    }

    /// <summary>
    /// Nhiệm vụ chạy nền
    /// </summary>
    public class ScheduleTask : BaseEntity
    {
        /// <summary>
        /// Tên nhiệm vụ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Thời gian mỗi lần chạy, tính bằng giây
        /// </summary>
        public long Seconds { get; set; }

        /// <summary>
        /// Loại nhiệm vụ chạy nền
        /// </summary>
        public ScheduleType Type { get; set; }

        /// <summary>
        /// Trạng thái bật
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Dừng lại khi lỗi
        /// </summary>
        public bool StopOnError { get; set; }

        /// <summary>
        /// Lần chạy cuối cùng
        /// </summary>
        public DateTime? LastStartTimed { get; set; }

        /// <summary>
        /// Lần kết thúc cuối cùng
        /// </summary>
        public DateTime? LastEndTimed { get; set; }

        /// <summary>
        /// Lần kết thúc thành công cuối cùng
        /// </summary>
        public DateTime? LastSuccessTimed { get; set; }
    }
}