using System;
using System.Collections.Generic;

namespace Pl.Core.Entities
{
    public class CmsMenu : BaseEntity
    {
        /// <summary>
        /// Vị trí hiển thị của menu
        /// adminsiderbar
        /// adminheader
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Trạng thái hiển thị
        /// 0 là không hiển thị
        /// 1 là hiển thị
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Lớp css của menu
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Tên menu
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Đường dẫn của menu
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Danh sách các quyền để có thê truy cập menu, phân cách nhau bằng 1 dấu ','
        /// </summary>
        public string RolesString { get; set; }

        /// <summary>
        /// Mảng roles
        /// </summary>
        public IEnumerable<string> RolesStringArray => string.IsNullOrEmpty(RolesString) ? Array.Empty<string>() : RolesString.Split(new char[] { ',' });

        /// <summary>
        /// Id menu cha
        /// </summary>
        public string ParentId { get; set; } = string.Empty;

        /// <summary>
        /// Thứ tự
        /// </summary>
        public int DisplayOrder { get; set; } = 1;

        /// <summary>
        /// Cách link mở ra khi click vào
        /// _blank
        /// _parent
        /// _self
        /// _top
        /// </summary>
        public string TargetType { get; set; } = "_blank";
    }
}