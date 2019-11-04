using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Pl.Core.Exceptions;
using Microsoft.Data.SqlClient;

namespace Pl.Caching
{
    public class SqlCacheService : IAsyncCacheService
    {
        #region Properties And Constructor

        /// <summary>
        /// Dịch vụ cache phân tán
        /// </summary>
        private readonly IDistributedCache _distributedCache;

        private readonly SqlServerCacheOptions _sqlServerCacheOptions;

        public SqlCacheService(IDistributedCache distributedCache, IOptions<SqlServerCacheOptions> option)
        {
            GuardClausesParameter.Null(option, nameof(option));

            _distributedCache = distributedCache;
            _sqlServerCacheOptions = option.Value;
        }

        #endregion Properties And Constructor

        #region Get Method

        /// <summary>
        /// Lấy đối tượng từ khóa cache và trả về với kiểu được chỉ định
        /// </summary>
        /// <typeparam name="TItem">Kiểu đối tượng cần nhận</typeparam>
        /// <param name="key">Khóa</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetByKeyAsync<TItem>(string key)
        {
            byte[] cacheData = await _distributedCache.GetAsync(key);
            if (cacheData != null)
            {
                return JsonSerializer.Deserialize<TItem>(Encoding.UTF8.GetString(cacheData));
            }
            return default;
        }

        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tượng, hàm bất đồng bộ
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory)
        {
            TItem cacheData = await GetByKeyAsync<TItem>(key);
            if (!EqualityComparer<TItem>.Default.Equals(cacheData, default))
            {
                return cacheData;
            }
            TItem funtionData = await factory();
            await SetValueAsync(key, funtionData);
            return funtionData;
        }

        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tượng, hàm bất đồng bộ
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <param name="time">Thời gian tồn tại cache tính bắng giây</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> factory, int time)
        {
            TItem cacheData = await GetByKeyAsync<TItem>(key);
            if (!EqualityComparer<TItem>.Default.Equals(cacheData, default))
            {
                return cacheData;
            }
            TItem funtionData = await factory();
            await SetValueAsync(key, funtionData, time);
            return funtionData;
        }

        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tuộng
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="paging"></param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetPagingOrCreateAsync<TItem>(string key, Paging paging, Func<Paging, Task<TItem>> factory)
        {
            GuardClausesParameter.Null(factory, nameof(factory));
            GuardClausesParameter.Null(paging, nameof(paging));

            var checkKey = key + paging.ToCacheKey();
            byte[] cacheData = _distributedCache.Get(checkKey);
            if (cacheData != null)
            {
                PagingCache<TItem> pagingCache = JsonSerializer.Deserialize<PagingCache<TItem>>(Encoding.UTF8.GetString(cacheData));
                paging.RowsCount = pagingCache.RowsCount;
                return pagingCache.Data;
            }
            TItem funtionData = await factory(paging);
            await SetValueAsync(checkKey, funtionData);
            return funtionData;
        }

        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tuộng
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="paging"></param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <param name="time">Thời gian cache tính bằng s</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetPagingOrCreateAsync<TItem>(string key, Paging paging, Func<Paging, Task<TItem>> factory, int time)
        {
            GuardClausesParameter.Null(factory, nameof(factory));
            GuardClausesParameter.Null(paging, nameof(paging));

            var checkKey = key + paging.ToCacheKey();
            byte[] cacheData = _distributedCache.Get(checkKey);
            if (cacheData != null)
            {
                PagingCache<TItem> pagingCache = JsonSerializer.Deserialize<PagingCache<TItem>>(Encoding.UTF8.GetString(cacheData));
                paging.RowsCount = pagingCache.RowsCount;
                return pagingCache.Data;
            }
            TItem funtionData = await factory(paging);
            await SetValueAsync(checkKey, funtionData, time);
            return funtionData;
        }

        #endregion Get Method

        #region Set Method

        /// <summary>
        /// Ghi một giá trị vào cache
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng cần set</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị</param>
        /// <returns>TItem</returns>
        public virtual async Task<TItem> SetValueAsync<TItem>(string key, TItem value)
        {
            await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
            return value;
        }

        /// <summary>
        /// Ghi một giá trị vào cache
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng cần set</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="value">Giá trị</param>
        /// <param name="time">Thời gian lưu trong cache tính bằng giây</param>
        /// <returns>Trả lại đối tượng TItem</returns>
        public virtual async Task<TItem> SetValueAsync<TItem>(string key, TItem value, int time)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(time));
            await _distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)), options);
            return value;
        }

        #endregion Set Method

        #region Remove Method

        /// <summary>
        /// Gỡ bỏ cache theo khóa cache bất đồng bộ
        /// </summary>
        /// <param name="key">Khóa cache</param>
        public virtual async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        /// <summary>
        /// Gõ bỏ cache theo mẫu khóa cache
        /// </summary>
        /// <param name="pattern">Mẫu khóa cache</param>
        public virtual async Task RemoveByPatternAsync(string pattern)
        {
            SqlConnection sqlConnection = new SqlConnection(_sqlServerCacheOptions.ConnectionString);
            try
            {
                using SqlCommand cmd = new SqlCommand($"DELETE [{_sqlServerCacheOptions.SchemaName}].[{_sqlServerCacheOptions.TableName}] WHERE Id like N'%' + @cacheKey + '%'")
                {
                    CommandType = CommandType.Text,
                    Connection = sqlConnection
                };
                cmd.Parameters.Add(new SqlParameter() { ParameterName = "@cacheKey", Value = pattern, SqlDbType = SqlDbType.VarChar, Size = 449 });
                sqlConnection.Open();
                await cmd.ExecuteScalarAsync();
                cmd.Dispose();
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// Xóa hết dữ liệu cache
        /// </summary>
        public virtual async Task ClearAsync()
        {
            SqlConnection sqlConnection = new SqlConnection(_sqlServerCacheOptions.ConnectionString);
            try
            {
                using SqlCommand cmd = new SqlCommand($"TRUNCATE TABLE [{_sqlServerCacheOptions.SchemaName}].[{_sqlServerCacheOptions.TableName}]")
                {
                    CommandType = CommandType.Text,
                    Connection = sqlConnection
                };
                sqlConnection.Open();
                await cmd.ExecuteScalarAsync();
                cmd.Dispose();
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        #endregion Remove Method
    }
}