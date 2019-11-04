using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface ISystemValueData : IAsyncRepository<SystemValue>
    {

        Task<IDataSourceResult<SystemValue>> GetSystemValuesAsync(int skip, int take, string key = "");

        Task<bool> SetStringByKeyAsync(string key, string value);

        Task<bool> CheckUpdateIsDuplicateKey(string key, string currentId);

        Task<string> GetStringByKeyOfCreateIfNotExistAsync(string key, string value, string description = "");

    }
}