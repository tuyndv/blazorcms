using System.Collections.Generic;
using System.Linq;

namespace Pl.Core.Entities
{
    public class UserGroup : BaseEntity
    {
        /// <summary>
        /// Tên nhóm
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Chuỗi phân quyền
        /// </summary>
        public string RolesString { get; set; }

        /// <summary>
        /// Lấy mảng quyền cho group
        /// </summary>
        public List<string> Roles => RolesString.Split(",").ToList();

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; } = 1;
    }
}