namespace Pl.Core.Entities
{
    public class UserMessage : BaseEntity
    {
        /// <summary>
        /// Receiver user and take notification icon
        /// </summary>
        public string ReceiverUserId { get; set; }

        /// <summary>
        /// Send user, if null of empty is system notification
        /// </summary>
        public string SendUserId { get; set; }

        /// <summary>
        /// Prict that user is readed this message
        /// </summary>
        public bool Readed { get; set; }

        /// <summary>
        /// font icon to display on notification arena
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Content of notification
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Link redirec user on click if sender need redirec user
        /// </summary>
        public string Link { get; set; }
    }
}
