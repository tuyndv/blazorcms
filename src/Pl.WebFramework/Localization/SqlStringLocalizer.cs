using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Pl.Core.Entities;
using Pl.System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Pl.WebFramework.Localization
{
    public class SqlStringLocalizer : IStringLocalizer
    {
        private static readonly Dictionary<string, string> _localizations = new Dictionary<string, string>();
        private readonly SqlLocalizationOptions _sqlLocalizationOptions;
        private readonly string _resourceType;
        private readonly IBackgroundTaskQueue _taskQueue;

        public SqlStringLocalizer(string resourceType, SqlLocalizationOptions sqlLocalizationOptions, IBackgroundTaskQueue taskQueue)
        {
            _sqlLocalizationOptions = sqlLocalizationOptions;
            _resourceType = resourceType;
            _taskQueue = taskQueue;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var resourceValue = GetResourceValue(name, out bool notSucceed);
                return new LocalizedString(name, resourceValue, notSucceed);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var resourceValue = GetResourceValue(name, out bool notSucceed);
                return new LocalizedString(name, string.Format(resourceValue, arguments), notSucceed);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            if (includeParentCultures)
            {
                return _localizations.Select(q => new LocalizedString(q.Key, q.Value));
            }
            var culture = CultureInfo.CurrentCulture.ToString();
            return _localizations.Where(q => q.Key.StartsWith($"{culture}-".ToLower())).Select(q => new LocalizedString(q.Key, q.Value, _sqlLocalizationOptions.CreateDbResourceIfNotFound, _resourceType));
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            if (culture == CultureInfo.CurrentCulture)
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// get resource value by name
        /// </summary>
        /// <param name="name">name key of resource</param>
        /// <param name="notFound">insert to db if not found</param>
        /// <returns></returns>
        private string GetResourceValue(string name, out bool notFound)
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var dicKey = $"{culture}-{_resourceType}-{name}".ToLower();
            if (_localizations.ContainsKey(dicKey))
            {
                notFound = true;
                return _localizations[dicKey];
            }
            else
            {
                notFound = false;
                _localizations.Add(dicKey, name);
                if (_sqlLocalizationOptions.CreateDbResourceIfNotFound)
                {
                    _taskQueue.QueueBackgroundWorkItem(async token =>
                    {
                        using var localizedDbContext = new SystemDbContext(new DbContextOptionsBuilder<SystemDbContext>().UseSqlServer(_sqlLocalizationOptions.ConnectionString).Options);
                        localizedDbContext.LanguageResources.Add(new LanguageResource()
                        {
                            Culture = culture,
                            Key = name,
                            Type = _resourceType,
                            Value = name
                        });
                        await localizedDbContext.SaveChangesAsync();
                    });
                }
                return name;
            }
        }

        /// <summary>
        /// Initialized language resources
        /// </summary>
        /// <param name="languageResources">List language resurce</param>
        public static void SetResourceLocalizations(List<LanguageResource> languageResources)
        {

            if (languageResources?.Count > 0)
            {
                languageResources.ForEach(q =>
                {
                    var key = $"{q.Culture}-{q.Type}-{q.Key}".ToLower();
                    if (!_localizations.ContainsKey(key))
                    {
                        _localizations.Add(key, q.Value);
                    }
                });
            }

        }
    }
}
