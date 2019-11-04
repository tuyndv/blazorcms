using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IMessageTemplateData : IAsyncRepository<MessageTemplate>
    {

        /// <summary>
        /// Lấy danh sách message template theo nhiều tiêu chí
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="name">Tên của thực ra khóa</param>
        /// <param name="title">Tiêu đề</param>
        /// <returns>Task IDataSourceResult MessageTemplate</returns>
        Task<IDataSourceResult<MessageTemplate>> GetMessageTemplatesAsync(int skip, int take, string name = "", string title = "");

        /// <summary>
        /// Kiểm tra xem mẫu tin nhắn này đã có trong hệ thống hay chưa
        /// </summary>
        /// <param name="name">Khóa của mẫu tin nhắn</param>
        /// <param name="id">Id trong trường hợp update</param>
        /// <returns>bool</returns>
        Task<bool> NameIsReadyAsync(string name, string id = null);

        /// <summary>
        /// get template by name
        /// </summary>
        /// <param name="name">Name of message template</param>
        /// <returns>Task MessageTemplate</returns>
        Task<MessageTemplate> GetByNameAsync(string name);

    }
}