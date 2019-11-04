using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace Pl.System
{
    public class ActivityLogData : EfRepository<ActivityLog>, IActivityLogData
    {
        public ActivityLogData(SystemDbContext systemDbContext) : base(systemDbContext)
        {

        }

        public async Task<IDataSourceResult<ActivityLog>> GetActivityLogsAsync(
            int skip,
            int take,
            string ipAddress = "",
            string message = "",
            string userId = null,
            ActivityTypeEnum? activityType = null,
            DateTime? startCreatedTime = null,
            DateTime? endCreatedTime = null)
        {
            BaseSpecification<ActivityLog> activityLogSpecification = new BaseSpecification<ActivityLog>(q =>
            (string.IsNullOrEmpty(message) || EF.Functions.Contains(q.Message, message))
            && (string.IsNullOrEmpty(userId) || q.UserId == userId)
            && (string.IsNullOrEmpty(ipAddress) || q.IpAddress == ipAddress)
            && (!startCreatedTime.HasValue || q.CreatedTime >= startCreatedTime)
            && (!endCreatedTime.HasValue || q.CreatedTime <= endCreatedTime)
            && (!activityType.HasValue || q.Type == activityType));
            activityLogSpecification.ApplyOrderByDescending(q => q.Id);
            activityLogSpecification.ApplySelector(q => new ActivityLog
            {
                Id = q.Id,
                CreatedTime = q.CreatedTime,
                UserId = q.UserId,
                Message = q.Message,
                Type = q.Type,
                ObjectType = q.ObjectType,
                IpAddress = q.IpAddress,
                Complete = q.Complete
            });
            activityLogSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(activityLogSpecification);
        }

        public async Task DeleteByUserIdAsync(string userId)
        {
            string queryDelete = $"DELETE {TableName} WHERE UserId = {userId}";
            await ExecuteSqlCommandAsync(queryDelete);
        }

        public async Task<Dictionary<string, int>> GetStatisticalByDayAsync(DateTime startDate, DateTime endDate, string userId = "")
        {
            Dictionary<string, int> listDateAndCount = new Dictionary<string, int>();
            using DbConnection conn = DbContext.Database.GetDbConnection();
            try
            {
                conn.Open();
                using DbCommand command = conn.CreateCommand();
                string whereQuery = " CreatedTime >= @startDate AND CreatedTime <= @endDate ";
                command.CommandText = $"SELECT CONVERT(DATE, CreatedTime) AS CreateDate , COUNT(UserId) AS ActionCount FROM {TableName} WHERE {whereQuery} GROUP BY CONVERT(DATE, CreatedTime)";
                DbParameter startDateParameter = command.CreateParameter();
                startDateParameter.ParameterName = "startDate";
                startDateParameter.Value = startDate;
                command.Parameters.Add(startDateParameter);

                DbParameter endDateParameter = command.CreateParameter();
                endDateParameter.ParameterName = "endDate";
                endDateParameter.Value = endDate;
                command.Parameters.Add(endDateParameter);

                if (!string.IsNullOrEmpty(userId))
                {
                    whereQuery += " AND UserId = @userId ";
                    DbParameter userIdParameter = command.CreateParameter();
                    userIdParameter.ParameterName = "userId";
                    userIdParameter.Value = endDate;
                    command.Parameters.Add(userIdParameter);
                }

                DbDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listDateAndCount.Add(reader.GetDateTime(0).ToString("dd/MM", CultureInfo.InvariantCulture), reader.GetInt32(1));
                    }
                }
                reader.Dispose();
                return listDateAndCount;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}