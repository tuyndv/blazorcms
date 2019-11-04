using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IEmailAccountData : IAsyncRepository<EmailAccount>
    {
        /// <summary>
        /// Hàm lấy các tài khản email theo nhiều tham số
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="useDefault">Là mặc đinh</param>
        /// <param name="name">Tên tài khoản email</param>
        /// <param name="email">Địa chỉ email</param>
        /// <param name="host">Server mail</param>
        /// <param name="userName">Email acount</param>
        /// <param name="port">Cổng</param>
        /// <returns>Task IDataSourceResult EmailAccount</returns>
        Task<IDataSourceResult<EmailAccount>> GetEmailAccountsAsync(int skip, int take, bool? useDefault = null, string name = "", string email = "", string host = "", string userName = "", int? port = null);

        /// <summary>
        /// Đặt tài khoản emai làm mặc định
        /// </summary>
        /// <param name="emailAcountId">Id tài khoản email cần sét</param>
        /// <returns>bool</returns>
        Task<bool> SetDefaultAsync(string emailAcountId);

        /// <summary>
        /// Lấy tài khoản email mặc định
        /// </summary>
        /// <returns>EmailAccount</returns>
        Task<EmailAccount> GetDefaultAsync();
    }
}