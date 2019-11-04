using Pl.Core;
using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using Pl.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Pl.System
{
    public class ObjectLocalizedData : EfRepository<ObjectLocalized>, IObjectLocalizedData
    {
        public ObjectLocalizedData(SystemDbContext systemDbContext) : base(systemDbContext)
        {

        }

        public async Task<string> GetLocalizedValueAsync(string languageCulture, string objectId, ObjectTypeEnum objectType, string propertyName, string stringIfNull = null)
        {
            ObjectLocalized objectLocalized = await FindNoTrackingAsync(q =>
                q.LanguageCulture == languageCulture &&
                q.ObjectId == objectId &&
                q.ObjectType == objectType &&
                q.PropertyName == propertyName);
            return objectLocalized?.LocalizedValue ?? stringIfNull;
        }

        public async Task<bool> DeleteLocalizedAsync(string languageCulture = null, string objectId = null, ObjectTypeEnum? objectTypeId = null, string propertyName = "")
        {
            string whereQuery = "";

            if (!string.IsNullOrEmpty(languageCulture))
            {
                whereQuery += $" LanguageCulture = '{languageCulture}' ";
            }

            if (!string.IsNullOrEmpty(languageCulture))
            {
                if (!string.IsNullOrWhiteSpace(whereQuery))
                {
                    whereQuery += " AND ";
                }

                whereQuery += $" ObjectId = '{objectId}' ";
            }

            if (objectTypeId.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(whereQuery))
                {
                    whereQuery += " AND ";
                }

                whereQuery += $" ObjectTypeId = {objectTypeId.Value} ";
            }

            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                if (!string.IsNullOrWhiteSpace(whereQuery))
                {
                    whereQuery += " AND ";
                }

                whereQuery += $" PropertyName = '{propertyName}' ";
            }

            string queryString = $"DELETE {TableName} WHERE {whereQuery}";
            return await ExecuteSqlCommandAsync(queryString);
        }

        public async Task<T> GetLocalizedStringAsync<T>(T entity, string languageCulture, ObjectTypeEnum objectType) where T : IBaseEntity
        {
            GuardClausesParameter.Null(entity, nameof(entity));
            var listLocalizedString = await FindAllNoTrackingAsync(q => q.LanguageCulture == languageCulture && q.ObjectType == objectType && q.ObjectId == entity.Id);
            foreach (ObjectLocalized item in listLocalizedString)
            {
                CoreUtility.SetProperty(entity, item.PropertyName, item.LocalizedValue);
            }
            return entity;
        }

        public async Task<IEnumerable<T>> GetLocalizedStringAsync<T>(IEnumerable<T> entitys, string languageCulture, ObjectTypeEnum objectType) where T : IBaseEntity
        {
            GuardClausesParameter.NullOrEmpty(entitys, nameof(entitys));
            var ids = string.Join(",", entitys.Select(q => $"'{q.Id}'"));
            var localizedStrings = await SqlQueryAsync($"SELECT * FROM {TableName} WHERE LanguageCulture = '{languageCulture}' AND ObjectType = {objectType} AND ObjectId IN ({ids})");
            foreach (var entity in entitys)
            {
                var itemLocalizedSting = localizedStrings.Where(q => q.ObjectId == entity.Id);
                foreach (ObjectLocalized item in itemLocalizedSting)
                {
                    CoreUtility.SetProperty(entity, item.PropertyName, item.LocalizedValue);
                }
            }
            return entitys;
        }

        public async Task<List<T>> GetLocalizedModelLocalAsync<T>(string objectId = null, ObjectTypeEnum? objectType = null) where T : IBaseEntity
        {
            List<T> listLocales = new List<T>();
            IOrderedQueryable<Language> listPublishLanguage = DbContext.Set<Language>().Where(q => !q.DisplayDefault).OrderBy(q => q.DisplayOrder);
            foreach (Language language in listPublishLanguage)
            {
                T entityModel = Activator.CreateInstance<T>();
                entityModel.Id = objectId;
                CoreUtility.SetProperty(entityModel, "LanguageCulture", language.Culture);
                if (!string.IsNullOrEmpty(objectId) && objectType.HasValue)
                {
                    listLocales.Add(await GetLocalizedStringAsync(entityModel, language.Culture, objectType.Value));
                }
                else
                {
                    listLocales.Add(entityModel);
                }
            }
            return listLocales;
        }

        public async Task<bool> SetLocalizedStringAsync<T>(T entity, string objectId, string languageCulture, ObjectTypeEnum objectType)
        {
            GuardClausesParameter.Null(entity, nameof(entity));

            foreach (PropertyInfo item in CoreUtility.GetPropertyList(entity))
            {
                if (item.Name != "LanguageId")
                {
                    string value = CoreUtility.GetProperty<string>(entity, item.Name);
                    await SaveLocalizedAsync(languageCulture, objectId, objectType, item.Name, value);
                }
            }
            return true;
        }

        public async Task SetLocalizedModelLocalAsync<T>(IEnumerable<T> listLocalizedData, string objectId, ObjectTypeEnum objectType)
        {
            GuardClausesParameter.NullOrEmpty(listLocalizedData, nameof(listLocalizedData));
            foreach (var istem in listLocalizedData)
            {
                await SetLocalizedStringAsync(istem, objectId, CoreUtility.GetProperty<string>(istem, "LanguageCulture"), objectType);
            }
        }

        public async Task<bool> SaveLocalizedAsync(string languageCulture, string objectId, ObjectTypeEnum objectType, string propertyName, string value)
        {
            ObjectLocalized objectLocalized = await FindAsync(q =>
                q.LanguageCulture == languageCulture &&
                q.ObjectId == objectId &&
                q.ObjectType == objectType &&
                q.PropertyName == propertyName);
            if (objectLocalized != null)
            {
                objectLocalized.LocalizedValue = value;
                return await UpdateAsync(objectLocalized);
            }
            else
            {
                ObjectLocalized insertEntity = new ObjectLocalized()
                {
                    LanguageCulture = languageCulture,
                    ObjectId = objectId,
                    ObjectType = objectType,
                    PropertyName = propertyName,
                    LocalizedValue = value,
                };
                return await InsertAsync(insertEntity);
            }
        }
    }
}