using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Pl.Logging
{
    public class SystemLogData : EfRepository<SystemLog>, ISystemLogData
    {
        public SystemLogData(DbLogDbContext dbLogDbContext) : base(dbLogDbContext)
        {

        }

        public async Task<IDataSourceResult<SystemLog>> GetErrorLogsAsync(
            int skip,
            int take,
            LogLevel? logLevelId = null,
            string shortMessage = "",
            string ipAdress = "",
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            BaseSpecification<SystemLog> errorLogSpecification = new BaseSpecification<SystemLog>(q =>
            (string.IsNullOrEmpty(shortMessage) || EF.Functions.Contains(q.Message, shortMessage))
            && (string.IsNullOrEmpty(ipAdress) || q.IpAddress == ipAdress)
            && (!startDate.HasValue || q.CreatedTime >= startDate)
            && (!endDate.HasValue || q.CreatedTime <= endDate)
            && (!logLevelId.HasValue || q.Level == logLevelId));
            errorLogSpecification.ApplyOrderByDescending(q => q.Id);
            errorLogSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(errorLogSpecification);
        }
    }
}