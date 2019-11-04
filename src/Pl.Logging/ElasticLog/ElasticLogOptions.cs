using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Pl.Logging
{
    public class ElasticLogOptions
    {
        /// <summary>
        /// Connection string to elatic servers
        /// </summary>
        public string Nodes { get; set; }

        /// <summary>
        /// finlter log by specify logleves
        /// </summary>
        public List<LogLevel> FilterLogLevels { get; } = new List<LogLevel>();

        /// <summary>
        /// action to create new config log
        /// </summary>
        /// <param name="nodes">List url elatic servers separate by ,</param>
        /// <param name="filterLogLevels">logleves</param>
        public void UseSettings(string nodes, params LogLevel[] filterLogLevels)
        {
            if (null != filterLogLevels)
            {
                FilterLogLevels.AddRange(filterLogLevels);
            }

            Nodes = nodes;
        }
    }
}
