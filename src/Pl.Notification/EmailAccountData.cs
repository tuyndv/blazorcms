using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pl.Notification
{
    public class EmailAccountData : EfRepository<EmailAccount>, IEmailAccountData
    {
        public EmailAccountData(NotificationDbContext notificationDbContext) : base(notificationDbContext)
        {

        }

        public virtual async Task<IDataSourceResult<EmailAccount>> GetEmailAccountsAsync(int skip, int take, bool? useDefault = null, string name = "", string email = "", string host = "", string userName = "", int? port = null)
        {
            BaseSpecification<EmailAccount> emailAccountSpecification = new BaseSpecification<EmailAccount>(q =>
            (string.IsNullOrEmpty(name) || EF.Functions.Contains(q.Name, name))
            && (string.IsNullOrEmpty(email) || q.Email == email)
            && (string.IsNullOrEmpty(host) || q.Host == host)
            && (string.IsNullOrEmpty(userName) || q.UserName == userName)
            && (!useDefault.HasValue || q.UseDefault == useDefault)
            && (!port.HasValue || q.Port == port));
            emailAccountSpecification.ApplyOrderByDescending(q => q.Id);
            emailAccountSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(emailAccountSpecification);
        }

        public virtual async Task<bool> SetDefaultAsync(string emailAcountId)
        {
            EmailAccount updateEntity = await FindAsync(emailAcountId);
            if (updateEntity == null)
            {
                return false;
            }

            foreach (EmailAccount item in await FindAllAsync(q => q.UseDefault))
            {
                item.UseDefault = false;
            }

            updateEntity.UseDefault = true;
            return await UpdateAsync(updateEntity);
        }

        public virtual async Task<EmailAccount> GetDefaultAsync()
        {
            return await FindAsync(q => q.UseDefault);
        }

    }
}