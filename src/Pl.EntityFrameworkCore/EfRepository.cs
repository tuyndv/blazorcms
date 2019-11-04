using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Pl.EntityFrameworkCore
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class, IBaseEntity
    {

        /// <summary>
        /// Get current db context
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// get table of this T
        /// </summary>
        /// <returns>string</returns>
        public string TableName => DbContext.GetTableName<T>();

        public EfRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        #region Check methods

        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            GuardClausesParameter.Null(predicate, nameof(predicate));
            return DbContext.Set<T>().AnyAsync(predicate);
        }

        #endregion Check methods

        #region Get methods

        public virtual async Task<T> FindAsync(string pk)
        {
            return await DbContext.Set<T>().FindAsync(pk);
        }

        public virtual async Task<T> FindNoTrackingAsync(string pk)
        {
            T entity = await DbContext.Set<T>().FindAsync(pk);
            if (entity != null)
            {
                DbContext.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public virtual Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            GuardClausesParameter.Null(predicate, nameof(predicate));
            return DbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public virtual Task<T> FindNoTrackingAsync(Expression<Func<T, bool>> predicate)
        {
            GuardClausesParameter.Null(predicate, nameof(predicate));
            return DbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region Counter methods

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            GuardClausesParameter.Null(predicate, nameof(predicate));
            return await DbContext.Set<T>().CountAsync(predicate);
        }

        public virtual async Task<int> CountAsync(ISpecification<T> specification)
        {
            GuardClausesParameter.Null(specification, nameof(specification));
            return await DbContext.Set<T>().GetSpecificationQuery(specification).CountAsync();
        }

        #endregion

        #region Listing methods

        public virtual async Task<IReadOnlyList<T>> FindAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> FindAllNoTrackingAsync()
        {
            return await DbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            GuardClausesParameter.Null(predicate, nameof(predicate));
            return await DbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> FindAllNoTrackingAsync(Expression<Func<T, bool>> predicate)
        {
            GuardClausesParameter.Null(predicate, nameof(predicate));
            return await DbContext.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> FindAllAsync(ISpecification<T> specification)
        {
            GuardClausesParameter.Null(specification, nameof(specification));
            return await DbContext.Set<T>().GetSpecificationQuery(specification).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> FindAllNoTrackingAsync(ISpecification<T> specification)
        {
            GuardClausesParameter.Null(specification, nameof(specification));
            return await DbContext.Set<T>().AsNoTracking().GetSpecificationQuery(specification).ToListAsync();
        }

        public virtual async Task<IDataSourceResult<T>> ToDataSourceResultAsync(ISpecification<T> specification)
        {
            return await DbContext.Set<T>().AsQueryable().ToDataSourceResultAsync(specification);
        }

        #endregion

        #region Insert methods

        public virtual async Task<bool> InsertAsync(T entity)
        {
            GuardClausesParameter.Null(entity, nameof(entity));
            await DbContext.Set<T>().AddAsync(entity);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> InsertAsync(IEnumerable<T> entities)
        {
            GuardClausesParameter.NullOrEmpty(entities, nameof(entities));
            await DbContext.Set<T>().AddRangeAsync(entities);
            return await DbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region Update methods

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            GuardClausesParameter.Null(entity, nameof(entity));
            DbContext.Entry(entity).State = EntityState.Modified;
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> UpdateAsync(IEnumerable<T> entities)
        {
            GuardClausesParameter.NullOrEmpty(entities, nameof(entities));

            foreach (T entity in entities)
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }
            return await DbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region Delete methods

        public virtual async Task<bool> TruncateAsync()
        {
            string query = $"TRUNCATE TABLE {TableName}";
            return await DbContext.Database.ExecuteSqlRawAsync(query) > 0;
        }

        public virtual async Task<bool> DeleteAsync(IEnumerable<T> entities)
        {
            GuardClausesParameter.NullOrEmpty(entities, nameof(entities));
            DbContext.Set<T>().RemoveRange(entities);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            GuardClausesParameter.Null(entity, nameof(entity));
            DbContext.Set<T>().Remove(entity);
            return await DbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeleteByKeysAsync(List<string> primaryKeys)
        {
            GuardClausesParameter.NullOrEmpty(primaryKeys, nameof(primaryKeys));
            return await ExecuteSqlCommandAsync($"DELETE {TableName} WHERE Id IN ({string.Join(",", primaryKeys)})");
        }

        public virtual async Task<bool> DeleteByKeyAsync(string primaryKey)
        {
            GuardClausesParameter.NullOrEmpty(primaryKey, nameof(primaryKey));
            return await ExecuteSqlCommandAsync($"DELETE {TableName} WHERE Id = {primaryKey}");
        }

        #endregion

        #region Query methods

        public virtual async Task<bool> ExecuteSqlCommandAsync(string queryCommand, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            GuardClausesParameter.NullOrEmpty(queryCommand, nameof(queryCommand));
            using IDbContextTransaction dbContextTransaction = await DbContext.Database.BeginTransactionAsync(isolationLevel);
            int dataQuery = await DbContext.Database.ExecuteSqlRawAsync(queryCommand);
            dbContextTransaction.Commit();
            return dataQuery > 0;
        }

        public virtual async Task<bool> ExecuteSqlCommandAsync(string queryCommand, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, params object[] parameters)
        {
            GuardClausesParameter.NullOrEmpty(queryCommand, nameof(queryCommand));
            using IDbContextTransaction dbContextTransaction = await DbContext.Database.BeginTransactionAsync(isolationLevel);
            int dataQuery = await DbContext.Database.ExecuteSqlRawAsync(queryCommand, default(CancellationToken), parameters);
            dbContextTransaction.Commit();
            return dataQuery > 0;
        }

        public virtual async Task<IReadOnlyList<T>> SqlQueryAsync(string queryCommand, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            GuardClausesParameter.NullOrEmpty(queryCommand, nameof(queryCommand));
            using IDbContextTransaction dbContextTransaction = await DbContext.Database.BeginTransactionAsync(isolationLevel);
            IQueryable<T> dataQuery = DbContext.Set<T>().FromSqlRaw(queryCommand);
            dbContextTransaction.Commit();
            return await dataQuery.ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> SqlQueryAsync(string queryCommand, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, params object[] parameters)
        {
            GuardClausesParameter.NullOrEmpty(queryCommand, nameof(queryCommand));
            using IDbContextTransaction dbContextTransaction = await DbContext.Database.BeginTransactionAsync(isolationLevel);
            IQueryable<T> dataQuery = DbContext.Set<T>().FromSqlRaw(queryCommand, parameters);
            dbContextTransaction.Commit();
            return await dataQuery.ToListAsync();
        }

        #endregion
    }
}