using Pl.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Get system working currency
        /// </summary>
        /// <param name="tagetCode">Taget currency code</param>
        /// <returns>Task Currency</returns>
        Task<Currency> GetWorkingCurrency(string tagetCode);

        /// <summary>
        /// Hàm này sẽ trả về định dạng tiền tệ của currency
        /// </summary>
        /// <param name="amount">Số tiền cần định dạng</param>
        /// <param name="targetCurrency">Loại tiền tệ cần hiển thị</param>
        /// <returns>string</returns>
        string CurrencyToString(decimal amount, Currency targetCurrency);

        /// <summary>
        /// Lấy danh sách tỉ giá trực tuyến, Hàm nay copy từ mạng
        /// </summary>
        /// <param name="exchangeRateCurrencyCode">Mã tỉ giá chính</param>
        /// <returns>list ExchangeRate</returns>
        Task<IList<ExchangeRate>> GetCurrencyLiveRatesAsync(string exchangeRateCurrencyCode);
    }
}