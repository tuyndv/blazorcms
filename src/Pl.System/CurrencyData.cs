using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pl.System
{
    public class CurrencyData : EfRepository<Currency>, ICurrencyData
    {
        public CurrencyData(SystemDbContext systemDbContext) : base(systemDbContext)
        {

        }

        public async Task<IDataSourceResult<Currency>> GetCurrenciesAsync(
            int skip,
            int take,
            string name = "",
            string currencyCode = "",
            bool? published = null,
            bool? isPrimaryExchange = null,
            bool? isPrimarySystem = null)
        {
            BaseSpecification<Currency> baseSpecification = new BaseSpecification<Currency>(q =>
            (string.IsNullOrEmpty(name) || EF.Functions.Contains(q.Name, name))
            && (string.IsNullOrEmpty(currencyCode) || q.CurrencyCode == currencyCode)
            && (!published.HasValue || q.Published == published)
            && (!isPrimaryExchange.HasValue || q.IsPrimaryExchange == isPrimaryExchange)
            && (!isPrimarySystem.HasValue || q.IsPrimarySystem == isPrimarySystem));
            baseSpecification.ApplyOrderBy(q => q.DisplayOrder);
            baseSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(baseSpecification);
        }

        public virtual async Task<bool> SetPrimaryAsync(string currencyId)
        {
            Currency updateEntity = await FindAsync(currencyId);
            if (updateEntity?.IsPrimarySystem != false)
            {
                return false;
            }

            foreach (Currency item in await FindAllAsync(q => q.IsPrimarySystem))
            {
                item.IsPrimarySystem = false;
            }

            updateEntity.IsPrimarySystem = true;
            return await UpdateAsync(updateEntity);
        }

        public virtual async Task<bool> SetExchangeRateAsync(string currencyId)
        {
            Currency updateEntity = await FindAsync(currencyId);
            if (updateEntity?.IsPrimaryExchange != false)
            {
                return false;
            }

            foreach (Currency item in await FindAllAsync(q => q.IsPrimaryExchange))
            {
                item.IsPrimaryExchange = false;
            }

            return await UpdateAsync(updateEntity);
        }

        public virtual async Task<bool> UpdateRateAsync(string currencyCode, decimal rate)
        {
            GuardClausesParameter.Null(currencyCode, nameof(currencyCode));

            Currency updateEntity = await FindAsync(q => q.CurrencyCode == currencyCode);
            if (updateEntity != null)
            {
                updateEntity.Rate = rate;
                return await UpdateAsync(updateEntity);
            }
            return false;
        }

    }
}