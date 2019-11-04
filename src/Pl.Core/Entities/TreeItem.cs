using System.Collections.Generic;

namespace Pl.Core.Entities
{
    public class TreeItem<T>
    {
        /// <summary>
        /// Đối tượng
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// danh sách các con của đối tượng
        /// </summary>
        public IEnumerable<TreeItem<T>> Children { get; set; }
    }
}