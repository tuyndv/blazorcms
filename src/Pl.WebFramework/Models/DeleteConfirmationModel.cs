namespace Pl.WebFramework.Models
{
    /// <summary>
    /// Mẫu from xóa entity
    /// </summary>
    public class DeleteConfirmationModel
    {
        /// <summary>
        /// Id entity
        /// </summary>
        public string ModelId { get; set; }

        /// <summary>
        /// Controller hứng xử lý
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Action Hứng xử lý
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Id modal
        /// </summary>
        public string ModalId { get; set; }
    }
}