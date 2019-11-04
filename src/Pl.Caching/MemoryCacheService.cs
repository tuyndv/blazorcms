using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pl.Caching
{
    public class MemoryCacheService : IAsyncCacheService, IDisposable
    {
        #region Properties And Constructor

        /// <summary>
        /// Dịch vụ memoryCache
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Tất cả các cache key
        /// </summary>
        protected static readonly ConcurrentDictionary<string, bool> allKeys = new ConcurrentDictionary<string, bool>();

        /// <summary>
        /// Xử lý khi cache hết hạn hoặc được xóa bỏ
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;


        public MemoryCacheService(IMemoryCache memoryCache)
        {
            GuardClausesParameter.Null(memoryCache, nameof(memoryCache));
            _memoryCache = memoryCache;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        #endregion Properties And Constructor

        #region Utilities

        /// <summary>
        /// Tạo một lựa chọn cache với các cấu hình nắng nghe sự kiện xóa cache hoặc hết hạn
        /// </summary>
        /// <param name="cacheTime">Thời gian cache tính bằng giây</param>
        /// <returns>MemoryCacheEntryOptions</returns>
        protected MemoryCacheEntryOptions GetMemoryCacheEntryOptions(int cacheTime = 60)
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
                .AddExpirationToken(new CancellationChangeToken(_cancellationTokenSource.Token))
                .RegisterPostEvictionCallback(PostEviction);
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheTime);
            return options;
        }

        /// <summary>
        /// Thêm một khóa cache vào danh sách
        /// </summary>
        /// <param name="key">Khóa</param>
        /// <returns>Trả lại key vừa add</returns>
        private static string AddKey(string key)
        {
            allKeys.TryAdd(key, true);
            return key;
        }

        /// <summary>
        /// Xóa một key trong tập danh sách
        /// </summary>
        /// <param name="key">Tên key cần xóa</param>
        /// <returns>Trả lại key vừa xóa</returns>
        private static string RemoveKey(string key)
        {
            TryRemoveKey(key);
            return key;
        }

        /// <summary>
        /// Thử gõ bỏ key ra khỏi danh sách hoặc đánh dấu cache không có với key này
        /// </summary>
        /// <param name="key">Khóa muốn gỡ bỏ</param>
        private static void TryRemoveKey(string key)
        {
            if (!allKeys.TryRemove(key, out _))
            {
                allKeys.TryUpdate(key, false, false);
            }
        }

        /// <summary>
        /// Xóa tất cả các key cache trong hệ thống
        /// </summary>
        private static void ClearKeys()
        {
            foreach (string key in allKeys.Where(p => !p.Value).Select(p => p.Key).ToList())
            {
                RemoveKey(key);
            }
        }

        /// <summary>
        /// Xử sự kiện khi cache hết hạn hoặc được gỡ ra
        /// </summary>
        /// <param name="key">Khóa</param>
        /// <param name="value">Giá trị của cache</param>
        /// <param name="reason">Lý do phát sinh sự kiện</param>
        /// <param name="state">Trạng thái</param>
        private void PostEviction(object key, object value, EvictionReason reason, object state)
        {
            if (reason == EvictionReason.Replaced)
            {
                return;
            }

            ClearKeys();

            TryRemoveKey(key.ToString());
        }

        #endregion Utilities

        #region Get Method

        /// <summary>
        /// Lấy đối tượng từ khóa cache và trả về với kiểu được chỉ định
        /// </summary>
        /// <typeparam name="TItem">Kiểu đối tượng cần nhận</typeparam>
        /// <param name="key">Khóa</param>
        /// <returns>TItem object</returns>
        public virtual async Task<TItem> GetByKeyAsync<TItem>(string key)
        {
            return await Task.Run(() =>
            {
                return _memoryCache.Get<TItem>(key);
            });

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
            GuardClausesParameter.Null(factory, nameof(factory));

            if (!_memoryCache.TryGetValue(key, out object result))
            {
                result = await factory();
                _memoryCache.Set(AddKey(key), result);
            }
            return (TItem)result;
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
            GuardClausesParameter.Null(factory, nameof(factory));

            if (!_memoryCache.TryGetValue(key, out object result))
            {
                result = await factory();
                _memoryCache.Set(AddKey(key), result, GetMemoryCacheEntryOptions(time));
            }
            return (TItem)result;
        }

        /// <summary>
        /// Lấy đối tượng từ khóa cache nếu chưa có cache sẽ gọi hàm lấy đối tuộng
        /// </summary>
        /// <typeparam name="TItem">Loại đối tượng</typeparam>
        /// <param name="key">Khóa cache</param>
        /// <param name="paging"></param>
        /// <param name="factory">Hàm lấy đối tượng nếu cache null</param>
        /// <returns>TItem object</returns>
        public async Task<TItem> GetPagingOrCreateAsync<TItem>(string key, Paging paging, Func<Paging, Task<TItem>> factory)
        {
            GuardClausesParameter.Null(factory, nameof(factory));
            GuardClausesParameter.Null(paging, nameof(paging));

            var checkKey = key + paging.ToCacheKey();
            if (!_memoryCache.TryGetValue(checkKey, out object result))
            {
                TItem data = await factory(paging);
                _memoryCache.Set(AddKey(checkKey), new PagingCache<TItem>() { Data = data, RowsCount = paging.RowsCount });
                return data;
            }
            PagingCache<TItem> pagingCache = (PagingCache<TItem>)result;
            paging.RowsCount = pagingCache.RowsCount;
            return pagingCache.Data;
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
            if (!_memoryCache.TryGetValue(checkKey, out object result))
            {
                TItem data = await factory(paging);
                _memoryCache.Set(AddKey(checkKey), new PagingCache<TItem>() { Data = data, RowsCount = paging.RowsCount }, GetMemoryCacheEntryOptions(time));
                return data;
            }
            PagingCache<TItem> pagingCache = (PagingCache<TItem>)result;
            paging.RowsCount = pagingCache.RowsCount;
            return pagingCache.Data;
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
            return await Task.Run(() =>
            {
                return _memoryCache.Set(AddKey(key), value, GetMemoryCacheEntryOptions());
            });
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
            return await Task.Run(() =>
            {
                return _memoryCache.Set(AddKey(key), value, GetMemoryCacheEntryOptions(time));
            });
        }

        #endregion Set Method

        #region Remove Method

        /// <summary>
        /// Gỡ bỏ cache theo khóa cache
        /// </summary>
        /// <param name="key">Khóa cache</param>
        public virtual void Remove(string key)
        {
            _memoryCache.Remove(RemoveKey(key));
        }

        /// <summary>
        /// Gỡ bỏ cache theo khóa cache bất đồng bộ
        /// </summary>
        /// <param name="key">Khóa cache</param>
        public virtual async Task RemoveAsync(string key)
        {
            await Task.Run(() => _memoryCache.Remove(RemoveKey(key)));
        }

        /// <summary>
        /// Gõ bỏ cache theo mẫu khóa cache
        /// </summary>
        /// <param name="pattern">Mẫu khóa cache</param>
        public virtual void RemoveByPattern(string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<string> matchesKeys = allKeys.Where(key => regex.IsMatch(key.Key)).Select(q => q.Key).ToList();
            matchesKeys.ForEach(Remove);
        }

        /// <summary>
        /// Gõ bỏ cache theo mẫu khóa cache
        /// </summary>
        /// <param name="pattern">Mẫu khóa cache</param>
        public virtual async Task RemoveByPatternAsync(string pattern)
        {
            await Task.Run(() =>
            {
                Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                List<string> matchesKeys = allKeys.Where(key => regex.IsMatch(key.Key)).Select(q => q.Key).ToList();
                matchesKeys.ForEach(key => Remove(key));
            });
        }

        /// <summary>
        /// Xóa hết dữ liệu cache
        /// </summary>
        public virtual async Task ClearAsync()
        {
            await Task.Run(() =>
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
            });
        }
        #endregion Remove Method

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellationTokenSource.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}