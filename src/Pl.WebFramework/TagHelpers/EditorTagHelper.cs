using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Pl.Core;
using System;
using System.Collections.Generic;

namespace Pl.WebFramework.TagHelpers
{
    [HtmlTargetElement("editor", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class EditorTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string DisabledAttributeName = "asp-disabled";
        private const string RequiredAttributeName = "asp-required";
        private const string ControlClassAttributeName = "asp-form-control-class";
        private const string TemplateAttributeName = "asp-template";
        private const string PostfixAttributeName = "asp-postfix";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Một expression chỉ ra model cần edit
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Trạng thái disable của control
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public bool IsDisabled { set; get; }

        /// <summary>
        /// Control có bắt buộc nhập  hay không
        /// </summary>
        [HtmlAttributeName(RequiredAttributeName)]
        public bool IsRequired { set; get; }

        /// <summary>
        /// Class css của control
        /// </summary>
        [HtmlAttributeName(ControlClassAttributeName)]
        public string RenderFormControlClass { set; get; }

        /// <summary>
        /// Chỉ định render template cho control nếu không muốn tự động gen
        /// </summary>
        [HtmlAttributeName(TemplateAttributeName)]
        public string Template { set; get; }

        /// <summary>
        /// Fix value cho control
        /// </summary>
        [HtmlAttributeName(PostfixAttributeName)]
        public string Postfix { set; get; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public EditorTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            output.SuppressOutput();

            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

            if (IsDisabled)
            {
                htmlAttributes.Add("disabled", "disabled");
            }

            if (IsRequired)
            {
                output.PreElement.SetHtmlContent("<div class='input-group input-group-required'>");
                output.PostElement.SetHtmlContent("<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>");
            }

            IViewContextAware viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            bool.TryParse(RenderFormControlClass, out bool renderFormControlClass);
            if ((string.IsNullOrEmpty(RenderFormControlClass) && For.Metadata.ModelType.Name.Equals("String")) || renderFormControlClass)
            {
                htmlAttributes.Add("class", "form-control");
            }

            IViewEngine viewEngine = CoreUtility.GetPrivateFieldValue(_htmlHelper, "_viewEngine") as IViewEngine;
            IViewBufferScope bufferScope = CoreUtility.GetPrivateFieldValue(_htmlHelper, "_bufferScope") as IViewBufferScope;
            TemplateBuilder templateBuilder = new TemplateBuilder(
                viewEngine,
                bufferScope,
                _htmlHelper.ViewContext,
                _htmlHelper.ViewData,
                For.ModelExplorer,
                For.Name,
                Template,
                readOnly: false,
                additionalViewData: new { htmlAttributes, postfix = Postfix });

            IHtmlContent htmlOutput = templateBuilder.Build();
            output.Content.SetHtmlContent(htmlOutput.ToHtmlString());
        }
    }
}