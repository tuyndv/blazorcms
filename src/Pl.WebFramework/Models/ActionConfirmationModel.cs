namespace Pl.WebFramework.Models
{
    /// <summary>
    /// Model hỗ trợ hỏi trước khi thực hiện một mvc action
    /// </summary>
    public class ActionConfirmationModel
    {
        /// <summary>
        /// Id ModalId hiển thị câu hỏi
        /// </summary>
        public string ModalId { get; set; }

        /// <summary>
        /// Nội dung câu hỏi
        /// </summary>
        public string ConfirmText { get; set; }
    }
}