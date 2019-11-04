namespace Pl.Core.Settings
{
    /// <summary>
    /// Tỏng  hợp connection
    /// </summary>
    public class Connections
    {
        /// <summary>
        /// Connection cho identity
        /// </summary>
        public string IdentityConnection { get; set; }

        /// <summary>
        /// System connection
        /// </summary>
        public string SystemConnection { get; set; }

        /// <summary>
        /// Notification connection
        /// </summary>
        public string NotificationConnection { get; set; }
    }
}