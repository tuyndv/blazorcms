using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface ILanguageData : IAsyncRepository<Language>
    {
        /// <summary>
        /// Hàm lấy danh sách ngôn ngữ theo nhiều tiêu chí
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="name">Tên ngôn ngữ</param>
        /// <param name="published">Trạng thái đăng</param>
        /// <param name="displayDefault">Hiển thị mặc định hay không</param>
        /// <returns>IQueryable Language</returns>
        Task<IDataSourceResult<Language>> GetLanguagesAsync(
            int skip,
            int take,
            string name = "",
            bool? published = null,
            bool? displayDefault = null);

        /// <summary>
        /// set a language to default system language
        /// </summary>
        /// <param name="languageId">Id language to set</param>
        /// <returns>bool</returns>
        Task<bool> SetDefaultAsync(string languageId);
    }
}