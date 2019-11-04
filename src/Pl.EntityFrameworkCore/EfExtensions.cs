using Microsoft.EntityFrameworkCore;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Pl.EntityFrameworkCore
{
    public static class EfExtensions
    {
        /// <summary>
        /// Get sql table name
        /// </summary>
        /// <typeparam name="T">Type of ef entity</typeparam>
        /// <param name="dbContext">ef db context</param>
        /// <returns></returns>
        public static string GetTableName<T>(this DbContext dbContext) where T : class, IBaseEntity
        {
            GuardClausesParameter.Null(dbContext, nameof(dbContext));
            return dbContext.Model.FindEntityType(typeof(T)).GetTableName();
        }

        /// <summary>
        /// Ap dụng điều kiện lọc vào cấu query
        /// </summary>
        /// <param name="inputQuery">Dữ liệu đầu vào</param>
        /// <param name="specification">Láy chi tiết dữ liệu</param>
        /// <returns>IQueryable T</returns>
        public static IQueryable<T> GetSpecificationQuery<T>(this IQueryable<T> inputQuery, ISpecification<T> specification) where T : class
        {
            GuardClausesParameter.Null(specification, nameof(specification));
            IQueryable<T> query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.Selector != null)
            {
                query = query.Select(specification.Selector);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }

        /// <summary>
        /// get IDataSourceResult from iquery
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="inputQuery">IQueryable of T</param>
        /// <param name="specification">a specification</param>
        /// <returns>Task IDataSourceResult T</returns>
        public static async Task<IDataSourceResult<T>> ToDataSourceResultAsync<T>(this IQueryable<T> inputQuery, ISpecification<T> specification) where T : class
        {
            GuardClausesParameter.Null(specification, nameof(specification));
            DataSourceResult<T> dataSourceResult = new DataSourceResult<T>();
            IQueryable<T> query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.Selector != null)
            {
                query = query.Select(specification.Selector);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            if (specification.IsPagingEnabled)
            {
                dataSourceResult.Total = query.Count();
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            dataSourceResult.Data = await query.ToListAsync();
            return dataSourceResult;
        }
    }
}
