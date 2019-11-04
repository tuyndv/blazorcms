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
    /// Hỗ trợ hỏi khi thực hiện một action
    /// </summary>
    [HtmlTargetElement("scriptconfirmation", Attributes = ButtonIdAttributeName + "," + CallBackFunctionName, TagStructure = TagStructure.WithoutEndTag)]
    public class ScriptConfirmationTagHelper : TagHelper
    {
        private const string ButtonIdAttributeName = "asp-button-id";
        private const string UiClassAttributeName = "asp-uiclass";
        private const string CallBackFunctionName = "asp-callback";
        private const string AdditionaConfirmText = "asp-additional-confirm";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Nhân gen html
        /// </summary>
        protected IHtmlGenerator Generator { get; set; }

        /// <summary>
        /// Id phần tử khi gọi confir
        /// </summary>
        [HtmlAttributeName(ButtonIdAttributeName)]
        public string ButtonId { get; set; }

        /// <summary>
        /// Hàm sẽ gọi đồng ý
        /// </summary>
        [HtmlAttributeName(CallBackFunctionName)]
        public string CallBackFunction { get; set; }

        /// <summary>
        /// Lớp css cho modal
        /// modal-info
        /// modal-danger
        /// modal-warning
        /// modal-success
        /// </summary>
        [HtmlAttributeName(UiClassAttributeName)]
        public string ModalUiClass { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Nội dung xác nhận nếu cần
        /// </summary>
        [HtmlAttributeName(AdditionaConfirmText)]
        public string ConfirmText { get; set; }

        public ScriptConfirmationTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper)
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

            string modalId = new HtmlString(ButtonId + "-action-confirmation").ToHtmlString();

            ScriptConfirmationModel scriptConfirmationModel = new ScriptConfirmationModel()
            {
                ModalId = modalId,
                ConfirmText = ConfirmText,
                CallBack = CallBackFunction
            };

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("id", modalId);
            output.Attributes.Add("class", $"modal {ModalUiClass} fade");
            output.Attributes.Add("tabindex", "-1");
            output.Attributes.Add("role", "dialog");
            output.Attributes.Add("aria-labelledby", $"{modalId}-title");
            output.Content.SetHtmlContent(await _htmlHelper.PartialAsync("ScriptConfirm", scriptConfirmationModel));

            TagBuilder script = new TagBuilder("script");
            script.InnerHtml.AppendHtml("$(document).ready(function () {" +
                                        $"$('#{ButtonId}').attr(\"data-toggle\", \"modal\").attr(\"data-target\", \"#{modalId}\");" +
                                        $"if($(\"#{ButtonId}\").attr(\"type\") == \"submit\")$(\"#{ButtonId}\").attr(\"type\", \"button\");" +
                                        "});");
            output.PostContent.SetHtmlContent(script.ToHtmlString());
        }
    }
}