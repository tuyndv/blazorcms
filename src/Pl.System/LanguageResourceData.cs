using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using System.Threading.Tasks;
using System.Text.Json;
using Pl.Core.Exceptions;
using System.Linq;
using System.Collections.Generic;
using Pl.EntityFrameworkCore;

namespace Pl.System
{
    public class LanguageResourceData : EfRepository<LanguageResource>, ILanguageResourceData
    {
        public LanguageResourceData(SystemDbContext systemDbContext) : base(systemDbContext)
        {

        }

        public async Task<IDataSourceResult<LanguageResource>> GetLanguageResources(
            int skip,
            int take,
            string culture = null,
            string key = null,
            string value = null)
        {
            BaseSpecification<LanguageResource> baseSpecification = new BaseSpecification<LanguageResource>(q =>
            (string.IsNullOrEmpty(value) || EF.Functions.Contains(q.Value, value))
            && (string.IsNullOrEmpty(key) || q.Key == key)
            && (string.IsNullOrEmpty(culture) || q.Culture == culture));
            baseSpecification.ApplyOrderByDescending(q => q.Id);
            baseSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(baseSpecification);
        }

        public async Task<bool> DeleteByCultureAsync(string culture)
        {
            string queryString = $"DELETE {TableName} WHERE Culture = '{culture}'";
            return await ExecuteSqlCommandAsync(queryString);
        }

        public async Task<string> ExportToJsonAsync(string culture)
        {
            GuardClausesParameter.Null(culture, nameof(culture));
            return JsonSerializer.Serialize((await FindAllNoTrackingAsync(q => q.Culture == culture)).Select(q => new
            {
                q.Culture,
                q.Key,
                q.Type,
                q.Value
            }), new JsonSerializerOptions() { WriteIndented = true });
        }

        public async Task<bool> ImportFromJsonAsync(string culture, string json)
        {
            GuardClausesParameter.Null(culture, nameof(culture));
            GuardClausesParameter.Null(json, nameof(json));

            List<LanguageResource> listInsert = JsonSerializer.Deserialize<List<LanguageResource>>(json);
            listInsert = listInsert.Where(q => !string.IsNullOrEmpty(q.Key) &&
                !string.IsNullOrEmpty(q.Culture) &&
                !string.IsNullOrEmpty(q.Type) &&
                !AnyAsync(c => c.Key == q.Key && c.Culture == q.Culture && c.Type == q.Type).Result).ToList();
            return await InsertAsync(listInsert);
        }
    }
}