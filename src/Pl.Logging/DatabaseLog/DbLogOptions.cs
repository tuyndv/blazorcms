using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Pl.Logging
{
    public class DbLogOptions
    {
        /// <summary>
        /// Connection string to db log
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// finlter log by specify logleves
        /// </summary>
        public List<LogLevel> FilterLogLevels { get; } = new List<LogLevel>();
        
        /// <summary>
        /// action to create new config log
        /// </summary>
        /// <param name="connectionString">db connection</param>
        /// <param name="filterLogLevels">logleves</param>
        public void UseSettings(string connectionString, params LogLevel[] filterLogLevels)
        {
            if (null != filterLogLevels)
            {
                FilterLogLevels.AddRange(filterLogLevels);
            }

            ConnectionString = connectionString;
        }
    }
}
