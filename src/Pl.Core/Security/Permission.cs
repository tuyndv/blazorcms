using System.Collections.Generic;

namespace Pl.Core.Security
{
    public class Permission
    {
        /// <summary>
        /// Create an instance of permission
        /// </summary>
        /// <param name="role">Role key</param>
        /// <param name="name">Resource name</param>
        /// <param name="permissions">Children permission</param>
        public Permission(string role, string name, IEnumerable<Permission> permissions = null)
        {
            Role = role;
            Name = name;
            Permissions = permissions;
        }

        /// <summary>
        /// Role key
        /// </summary>
        public string Role { get; private set; }

        /// <summary>
        /// Resource name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Children of thi permission
        /// </summary>
        public IEnumerable<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
