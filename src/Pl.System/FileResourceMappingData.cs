using Pl.Core;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pl.System
{
    public class FileResourceMappingData : EfRepository<FileResourceMapping>, IFileResourceMappingData
    {
        public FileResourceMappingData(SystemDbContext systemDbContext) : base(systemDbContext)
        {
        }

        public async Task<IDataSourceResult<FileResourceMapping>> GetMediaMappingsAsync(int skip, int take, string fileResourceId = null, string objectId = null, ObjectTypeEnum? objectTypeId = null, bool? published = null)
        {
            BaseSpecification<FileResourceMapping> baseSpecification = new BaseSpecification<FileResourceMapping>(q =>
            (!published.HasValue || q.Published == published)
            && (!objectTypeId.HasValue || q.ObjectType == objectTypeId)
            && (string.IsNullOrEmpty(objectId) || q.ObjectId == objectId)
            && (string.IsNullOrEmpty(fileResourceId) || q.FileResourceId == fileResourceId));
            baseSpecification.ApplyOrderBy(q => q.DisplayOrder);
            baseSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(baseSpecification);
        }
    }
}