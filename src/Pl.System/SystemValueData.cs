using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Pl.System
{
    public class SystemValueData : EfRepository<SystemValue>, ISystemValueData
    {
        public SystemValueData(SystemDbContext systemDbContext) : base(systemDbContext)
        {

        }

        /// <summary>
        /// Hàm lấy danh sách giá trị config hệ thống theo key
        /// </summary>
        /// <param name="skip">Số bản ghi bỏ qua</param>
        /// <param name="take">Số bản ghi cần lấy</param>
        /// <param name="key">Key code</param>
        /// <returns>IDataSourceResult</returns>
        public async Task<IDataSourceResult<SystemValue>> GetSystemValuesAsync(int skip, int take, string key = "")
        {
            BaseSpecification<SystemValue> systemValueSpecification = new BaseSpecification<SystemValue>(q =>
            (string.IsNullOrEmpty(key) || EF.Functions.Contains(q.Key, key)));
            systemValueSpecification.ApplyOrderByDescending(q => q.Id);
            systemValueSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(systemValueSpecification);
        }

        /// <summary>
        /// Set một giá trị theo key và mô tả
        /// </summary>
        /// <param name="key">Khóa</param>
        /// <param name="value">Giá trị</param>
        /// <returns>Nếu đã tồn tại thì sẽ update</returns>
        public async Task<bool> SetStringByKeyAsync(string key, string value)
        {
            GuardClausesParameter.Null(key, nameof(key));
            GuardClausesParameter.Null(value, nameof(value));

            SystemValue systemKeyValue = await FindAsync(q => q.Key == key);
            if (systemKeyValue != null)
            {
                systemKeyValue.Value = value;
                return await UpdateAsync(systemKeyValue);
            }
            systemKeyValue = new SystemValue() { Key = key, Value = value };
            return await InsertAsync(systemKeyValue);
        }

        /// <summary>
        /// Kiểm tra update có bị trùng key hay không
        /// </summary>
        /// <param name="key">Khóa giá trị</param>
        /// <param name="currentId">Id hiện tại cần check</param>
        /// <returns>bool</returns>
        public async Task<bool> CheckUpdateIsDuplicateKey(string key, string currentId)
        {
            var duplicateItem = await FindAsync(q => q.Key == key);
            return duplicateItem != null && duplicateItem.Id != currentId;
        }

        /// <summary>
        /// Lấy value theo key và tạo mới nếu chưa có
        /// </summary>
        /// <param name="key">Khóa</param>
        /// <param name="value">Giá trị</param>
        /// <param name="description">Mô tả</param>
        /// <returns>string value</returns>
        public async Task<string> GetStringByKeyOfCreateIfNotExistAsync(string key, string value, string description = "")
        {
            SystemValue _systemKeyValue = await FindAsync(q => q.Key == key);
            if (_systemKeyValue != null)
            {
                return _systemKeyValue.Value;
            }

            _systemKeyValue = new SystemValue() { Key = key, Value = value, Description = description };
            await InsertAsync(_systemKeyValue);
            return value;
        }
    }
}