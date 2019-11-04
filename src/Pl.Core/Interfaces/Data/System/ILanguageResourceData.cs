using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface ILanguageResourceData : IAsyncRepository<LanguageResource>
    {
        /// <summary>
        /// Lấy giá trị ngôn ngữ theo nhiều tham số
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="culture">Language culture</param>
        /// <param name="key">Khóa ngôn ngữ</param>
        /// <param name="value">Giá tị ngôn ngữ</param>
        /// <returns>IDataSourceResult LanguageResource</returns>
        Task<IDataSourceResult<LanguageResource>> GetLanguageResources(
            int skip,
            int take,
            string culture = null,
            string key = null,
            string value = null);

        /// <summary>
        /// Delete resource by culture
        /// </summary>
        /// <param name="culture">language culture</param>
        /// <returns>bool</returns>
        Task<bool> DeleteByCultureAsync(string culture);

        /// <summary>
        /// export resource data to json
        /// </summary>
        /// <param name="culture">culture to export</param>
        /// <returns>string</returns>
        Task<string> ExportToJsonAsync(string culture);

        /// <summary>
        /// Import languagre resource from json data
        /// </summary>
        /// <param name="culture">language culture</param>
        /// <param name="json">json file content</param>
        /// <returns>Task bool</returns>
        Task<bool> ImportFromJsonAsync(string culture, string json);
    }
}