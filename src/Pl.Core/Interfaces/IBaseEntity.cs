using System;

namespace Pl.Core.Interfaces
{

    public interface IBaseEntity
    {

        /// <summary>
        /// Identity key
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Time created of object
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// Last time updated object
        /// </summary>
        DateTime UpdatedTime { get; set; }

    }
}