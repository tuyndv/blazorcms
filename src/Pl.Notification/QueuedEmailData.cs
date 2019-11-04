using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Pl.Notification
{
    public class QueuedEmailData : EfRepository<QueuedEmail>, IQueuedEmailData
    {
        public QueuedEmailData(NotificationDbContext notificationDbContext) : base(notificationDbContext)
        {

        }

        /// <summary>
        /// Lấy email trong queued theo nhiều tham số
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="title">Tiêu đề email</param>
        /// <param name="to">Gửi đến</param>
        /// <param name="submitted">Đã gửi hay chưa</param>
        /// <param name="emailAccouantId">Tài khoản gửi. Chỉ có khi đã gửi</param>
        /// <param name="fromDate">Từ Ngày tạo email trong queued</param>
        /// <param name="toDate">Đến Ngày tạo email trong queued</param>
        /// <returns>Task IDataSourceResult QueuedEmail</returns>
        public virtual async Task<IDataSourceResult<QueuedEmail>> GetQueuedEmailsAsync(
            int skip, 
            int take, 
            string title = "", 
            string toEmail = "", 
            bool? submitted = null, 
            string emailAccouantId = null, 
            DateTime? fromDate = null, 
            DateTime? toDate = null)
        {
            BaseSpecification<QueuedEmail> queuedEmailSpecification = new BaseSpecification<QueuedEmail>(q =>
            (string.IsNullOrEmpty(title) || EF.Functions.Contains(q.Title, title))
            && (string.IsNullOrEmpty(toEmail) || q.To == toEmail)
            && (!submitted.HasValue || (submitted == true && q.SendTime != null) || (submitted != true && q.SendTime == null))
            && (string.IsNullOrEmpty(emailAccouantId) || q.EmailAccountId == emailAccouantId)
            && (!fromDate.HasValue || q.CreatedTime >= fromDate)
            && (!toDate.HasValue || q.CreatedTime <= toDate));
            queuedEmailSpecification.ApplyOrderByDescending(q => q.Id);
            queuedEmailSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(queuedEmailSpecification);
        }

        /// <summary>
        /// Update queue email to null email account send
        /// </summary>
        /// <param name="emailAccountId">Email Account Id</param>
        public virtual async Task SetNullEmailAccountAsync(long emailAccountId)
        {
            string queryDeleteString = $"UPDATE {TableName} SET EmailAccountId = NULL WHERE EmailAccountId = '{emailAccountId}'";
            await ExecuteSqlCommandAsync(queryDeleteString);
        }
    }
}