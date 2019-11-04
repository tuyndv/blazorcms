using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Pl.Core.Services
{
    public class CurrencyService : ICurrencyService
    {
        #region Properties And Constructor

        private readonly ICurrencyData _currencyData;
        private readonly IAsyncCacheService _cacheService;

        public CurrencyService(
            IAsyncCacheService cacheService,
            ICurrencyData currencyData)
        {
            _currencyData = currencyData;
            _cacheService = cacheService;
        }

        #endregion Properties And Constructor

        public virtual async Task<Currency> GetWorkingCurrency(string tagetCode)
        {
            string cacheKey = $"{CoreConstants.CurrencyCacheKey}_getworkingcurrency_{tagetCode}";
            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                Currency workingCurrentcy = null;
                if (!string.IsNullOrWhiteSpace(tagetCode))
                {
                    workingCurrentcy = await _currencyData.FindAsync(q => q.CurrencyCode == tagetCode);
                }
                return workingCurrentcy ?? await _currencyData.FindAsync(q => q.IsPrimarySystem);
            }, CoreConstants.DefaultCacheTime);
        }

        public virtual string CurrencyToString(decimal amount, Currency targetCurrency)
        {
            GuardClausesParameter.Null(targetCurrency, nameof(targetCurrency));

            if (!string.IsNullOrEmpty(targetCurrency.CustomFormatting))
            {
                return amount.ToString(targetCurrency.CustomFormatting, CultureInfo.InvariantCulture);
            }
            else
            {
                if (!string.IsNullOrEmpty(targetCurrency.Culture))
                {
                    CultureInfo format = new CultureInfo(targetCurrency.Culture);
                    return amount.ToString("C", format);
                }
                else
                {
                    return $"{amount.ToString("N", CultureInfo.InvariantCulture)} ({targetCurrency.CurrencyCode})";
                }
            }
        }

        public virtual async Task<IList<ExchangeRate>> GetCurrencyLiveRatesAsync(string exchangeRateCurrencyCode)
        {
            if (exchangeRateCurrencyCode == null)
            {
                throw new ArgumentNullException(nameof(exchangeRateCurrencyCode));
            }

            List<ExchangeRate> ratesToEuro = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    CurrencyCode = "EUR",
                    Rate = 1,
                    UpdatedOn = DateTime.UtcNow
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml"));
            using (WebResponse response = await request.GetResponseAsync())
            {
                XmlDocument document = new XmlDocument();
                document.Load(response.GetResponseStream());

                XmlNamespaceManager namespaces = new XmlNamespaceManager(document.NameTable);
                namespaces.AddNamespace("ns", "http://www.ecb.int/vocabulary/2002-08-01/eurofxref");
                namespaces.AddNamespace("gesmes", "http://www.gesmes.org/xml/2002-08-01");

                XmlNode dailyRates = document.SelectSingleNode("gesmes:Envelope/ns:Cube/ns:Cube", namespaces);
                if (!DateTime.TryParseExact(dailyRates.Attributes["time"].Value, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime updateDate))
                {
                    updateDate = DateTime.UtcNow;
                }

                foreach (XmlNode currency in dailyRates.ChildNodes)
                {
                    if (!decimal.TryParse(currency.Attributes["rate"].Value, out decimal currencyRate))
                    {
                        continue;
                    }

                    ratesToEuro.Add(new ExchangeRate()
                    {
                        CurrencyCode = currency.Attributes["currency"].Value,
                        Rate = currencyRate,
                        UpdatedOn = updateDate
                    });
                }
            }

            if (exchangeRateCurrencyCode.Equals("eur", StringComparison.InvariantCultureIgnoreCase))
            {
                return ratesToEuro;
            }

            ExchangeRate exchangeRateCurrency = ratesToEuro.Find(rate => rate.CurrencyCode.Equals(exchangeRateCurrencyCode, StringComparison.InvariantCultureIgnoreCase));
            if (exchangeRateCurrency == null)
            {
                throw new Exception($"The {exchangeRateCurrencyCode} don't have in list currency");
            }

            return ratesToEuro.Select(rate => new ExchangeRate
            {
                CurrencyCode = rate.CurrencyCode,
                Rate = Math.Round(rate.Rate / exchangeRateCurrency.Rate, 4),
                UpdatedOn = rate.UpdatedOn
            }).ToList();
        }

    }
}