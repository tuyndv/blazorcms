using Pl.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IQueuedEmailData : IAsyncRepository<QueuedEmail>
    {
        /// <summary>
        /// Update queue email to null email account send
        /// </summary>
        /// <param name="emailAccountId">Email Account Id</param>
        Task SetNullEmailAccountAsync(long emailAccountId);

        /// <summary>
        /// Lấy email trong queued theo nhiều tham số
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="title">Tiêu đề email</param>
        /// <param name="toEmail">Gửi đến</param>
        /// <param name="submitted">Đã gửi hay chưa</param>
        /// <param name="emailAccouantId">Tài khoản gửi. Chỉ có khi đã gửi</param>
        /// <param name="fromDate">Từ Ngày tạo email trong queued</param>
        /// <param name="toDate">Đến Ngày tạo email trong queued</param>
        /// <returns>Task IDataSourceResult QueuedEmail</returns>
        Task<IDataSourceResult<QueuedEmail>> GetQueuedEmailsAsync(
            int skip,
            int take,
            string title = "",
            string toEmail = "",
            bool? submitted = null,
            string emailAccouantId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);
    }
}