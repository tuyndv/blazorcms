using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Pl.Core;
using Pl.WebFramework.Models;
using System;
using System.Threading.Tasks;

namespace Pl.WebFramework.TagHelpers
{
    /// <summary>
    /// Hỗ trợ tạo modal xóa đối tượng
    /// </summary>
    [HtmlTargetElement("deleteconfirmation", Attributes = ModelIdAttributeName + "," + ButtonIdAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class DeleteConfirmationTagHelper : TagHelper
    {
        private const string ModelIdAttributeName = "asp-model-id";
        private const string ButtonIdAttributeName = "asp-button-id";
        private const string ActionAttributeName = "asp-action";

        private readonly IHtmlHelper _htmlHelper;

        protected IHtmlGenerator Generator { get; set; }

        /// <summary>
        /// Id của đối tượng cần xóa
        /// </summary>
        [HtmlAttributeName(ModelIdAttributeName)]
        public string ModelId { get; set; }

        /// <summary>
        /// Nút xóa
        /// </summary>
        [HtmlAttributeName(ButtonIdAttributeName)]
        public string ButtonId { get; set; }

        /// <summary>
        /// Tên action xóa
        /// </summary>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public DeleteConfirmationTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper)
        {
            Generator = generator;
            _htmlHelper = htmlHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            IViewContextAware viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            if (string.IsNullOrEmpty(Action))
            {
                Action = "Delete";
            }

            string modelName = _htmlHelper.ViewData.ModelMetadata.ModelType.Name.ToLower();
            if (!string.IsNullOrEmpty(Action))
            {
                modelName += "-" + Action;
            }

            string modalId = new HtmlString(modelName + "-delete-confirmation").ToHtmlString();

            if (!string.IsNullOrEmpty(ModelId))
            {
                DeleteConfirmationModel deleteConfirmationModel = new DeleteConfirmationModel
                {
                    ModelId = ModelId,
                    Controller = _htmlHelper.ViewContext.RouteData.Values["controller"].ToString(),
                    Action = Action,
                    ModalId = modalId
                };

                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Add("id", modalId);
                output.Attributes.Add("class", "modal modal-danger fade");
                output.Attributes.Add("tabindex", "-1");
                output.Attributes.Add("role", "dialog");
                output.Attributes.Add("aria-labelledby", $"{modalId}-title");
                output.Content.SetHtmlContent(await _htmlHelper.PartialAsync("Delete", deleteConfirmationModel));

                TagBuilder script = new TagBuilder("script");
                script.InnerHtml.AppendHtml("$(document).ready(function () {" + $"$('#{ButtonId}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{modalId}\")" + "});");
                output.PostContent.SetHtmlContent(script.ToHtmlString());
            }
        }
    }
}