namespace Pl.Core.Entities
{
    public enum SystemTemplateName
    {
        /// <summary>
        /// Key string to get template for email welcome user register system
        /// </summary>
        WelcomeToSystem,

        /// <summary>
        /// key get User register and active account by email activer
        /// </summary>
        RegisterActiveByEmail,

        /// <summary>
        /// The key get message template help user recover your passowrd
        /// </summary>
        ForgotPassword
    }

    /// <summary>
    /// Mẫu tin nhắn
    /// </summary>
    public class MessageTemplate : BaseEntity
    {
        /// <summary>
        /// Tên của mẫu tin nhắn
        /// Tên này sẽ dùng để làm khóa truy vấn vào
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tiêu đề của mẫu tin nhắn
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Nội dung của mẫu tin nhắn
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Description for message and list key replate string for this message ex.
        /// [UserName] => will replate by user display name
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// if true the template can't edit name, can't delete
        /// </summary>
        public bool IsSystemTemplate { get; set; }
    }
}