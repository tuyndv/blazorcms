using Pl.Core.Interfaces;
using System;
using System.Text.Json;

namespace Pl.Core.Entities
{
    /// <summary>
    /// Lớp cơ bản của một entity trong hệ thống.
    /// </summary>
    public class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Khóa chính của entity
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Thời điểm tạo mới entity
        /// </summary>
        public virtual DateTime CreatedTime { get; set; }

        /// <summary>
        /// Thời điểm cập nhập entity lần cuối cùng
        /// </summary>
        public virtual DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Hàm trả về object dạng json
        /// </summary>
        public virtual string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true });
        }
    }
}