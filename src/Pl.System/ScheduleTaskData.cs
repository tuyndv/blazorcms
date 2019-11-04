using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Interfaces;
using Pl.Core.Specifications;
using Pl.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Pl.System
{
    public class ScheduleTaskData : EfRepository<ScheduleTask>, IScheduleTaskData
    {
        public ScheduleTaskData(SystemDbContext systemDbContext) : base(systemDbContext)
        {

        }

        public async Task<IDataSourceResult<ScheduleTask>> GetScheduleTasksAsync(int skip, int take, string name = "", ScheduleType? type = null, bool? enabled = null, bool? stopOnError = null)
        {
            BaseSpecification<ScheduleTask> baseSpecification = new BaseSpecification<ScheduleTask>(q =>
            (string.IsNullOrEmpty(name) || EF.Functions.Contains(q.Name, name))
            && (!type.HasValue || q.Type == type)
            && (!enabled.HasValue || q.Enabled == enabled)
            && (!stopOnError.HasValue || q.StopOnError == stopOnError));
            baseSpecification.ApplyOrderByDescending(q => q.Id);
            baseSpecification.ApplyPaging(skip, take);
            return await ToDataSourceResultAsync(baseSpecification);
        }
    }
}