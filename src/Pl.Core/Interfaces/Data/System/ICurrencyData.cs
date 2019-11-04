using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface ICurrencyData : IAsyncRepository<Currency>
    {
        /// <summary>
        /// Lấy danh sách và bỏ theo dõi thay đổi của các đối tượng lấy ra
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="name">Thông tin phân trang</param>
        /// <param name="name">Tên tiền tệ</param>
        /// <param name="currencyCode">Mã tiền tệ</param>
        /// <param name="published">Trang thái đăng</param>
        /// <param name="isPrimaryExchange">là tỉ giá hối đoái</param>
        /// <param name="isPrimarySystem">Là tiền tệ chính</param>
        /// <returns>IQueryable Currency </returns>
        Task<IDataSourceResult<Currency>> GetCurrenciesAsync(
            int skip,
            int take,
            string name = "",
            string currencyCode = "",
            bool? published = null,
            bool? isPrimaryExchange = null,
            bool? isPrimarySystem = null);

        /// <summary>
        /// set primary currency in system
        /// </summary>
        /// <param name="currencyId">currency id</param>
        /// <returns>Task bool</returns>
        Task<bool> SetPrimaryAsync(string currencyId);

        /// <summary>
        /// set default exchange rate currency
        /// </summary>
        /// <param name="currencyId">currency id</param>
        /// <returns>Task bool</returns>
        Task<bool> SetExchangeRateAsync(string currencyId);

        /// <summary>
        /// update exchange rate of currency by code
        /// </summary>
        /// <param name="currencyCode">currency code to update</param>
        /// <param name="rate">rate value</param>
        /// <returns>Task bool</returns>
        Task<bool> UpdateRateAsync(string currencyCode, decimal rate);
    }
}