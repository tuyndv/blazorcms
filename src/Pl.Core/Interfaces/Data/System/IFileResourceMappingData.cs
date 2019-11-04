using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface IFileResourceMappingData : IAsyncRepository<FileResourceMapping>
    {
        /// <summary>
        /// <summary>
        /// Hàm lấy mapping theo nhiều tham số
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="fileResourceId">Id media</param>
        /// <param name="objectId">Id đối tượng</param>
        /// <param name="objectTypeId">Id loại đối tượng</param>
        /// <param name="published">Trạng thái đăng</param>
        /// <returns>IDataSourceResult FileResourceMapping</returns>
        Task<IDataSourceResult<FileResourceMapping>> GetMediaMappingsAsync(int skip, int take, string fileResourceId = null, string objectId = null, ObjectTypeEnum? objectTypeId = null, bool? published = null);
    }
}