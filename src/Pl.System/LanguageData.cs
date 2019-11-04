using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pl.System
{
    public class LanguageData : EfRepository<Language>, ILanguageData
    {
        public LanguageData(SystemDbContext systemDbContext) : base(systemDbContext)
        {

        }

        public async Task<IDataSourceResult<Language>> GetLanguagesAsync(
            int skip,
            int take,
            string name = "",
            bool? published = null,
            bool? displayDefault = null)
        {
            BaseSpecification<Language> baseSpecification = new BaseSpecification<Language>(q =>
            (string.IsNullOrEmpty(name) || EF.Functions.Contains(q.Name, name))
            && (!displayDefault.HasValue || q.DisplayDefault == displayDefault)
            && (!published.HasValue || q.Published == published));
            baseSpecification.ApplyOrderByDescending(q => q.Id);
            baseSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(baseSpecification);
        }

        public async Task<bool> SetDefaultAsync(string languageId)
        {
            Language updateEntity = await FindAsync(languageId);
            if (updateEntity?.DisplayDefault == true)
            {
                return false;
            }

            foreach (Language item in await FindAllAsync(q => q.DisplayDefault))
            {
                item.DisplayDefault = false;
            }

            updateEntity.DisplayDefault = true;
            return await UpdateAsync(updateEntity);
        }
    }
}