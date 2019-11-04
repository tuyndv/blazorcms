namespace Pl.Core.Entities
{
    /// <summary>
    /// Liên kết giữa người dùng và nhóm người dùng
    /// </summary>
    public class UserGroupMapping : BaseEntity
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Id nhóm
        /// </summary>
        public string UserGroupId { get; set; }

    }
}