using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Pl.Notification
{
    public class MessageTemplateData : EfRepository<MessageTemplate>, IMessageTemplateData
    {
        public MessageTemplateData(NotificationDbContext notificationDbContext) : base(notificationDbContext)
        {

        }

        public virtual async Task<IDataSourceResult<MessageTemplate>> GetMessageTemplatesAsync(int skip, int take, string name = "", string title = "")
        {
            BaseSpecification<MessageTemplate> messageTemplateSpecification = new BaseSpecification<MessageTemplate>(q =>
            (string.IsNullOrEmpty(name) || EF.Functions.Contains(q.Name, name))
            && (string.IsNullOrEmpty(title) || EF.Functions.Contains(q.Title, title)));
            messageTemplateSpecification.ApplyOrderByDescending(q => q.Id);
            messageTemplateSpecification.ApplySelector(q => new MessageTemplate()
            {
                Id = q.Id,
                Name = q.Name,
                Title = q.Title,
                UpdatedTime = q.UpdatedTime
            });
            messageTemplateSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(messageTemplateSpecification);
        }

        public virtual async Task<bool> NameIsReadyAsync(string name, string id = null)
        {
            MessageTemplate messageTemplate = await FindAsync(q => q.Name == name);
            if (messageTemplate != null)
            {
                return !string.IsNullOrEmpty(id) && id == messageTemplate.Id;
            }
            else
            {
                return true;
            }
        }

        public virtual async Task<MessageTemplate> GetByNameAsync(string name)
        {
            return await FindAsync(q => q.Name == name);
        }
    }
}