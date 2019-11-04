using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pl.WebFramework.TagHelpers
{
    [HtmlTargetElement("cmstextarea", Attributes = ForAttributeName)]
    public class CmsTextAreaTagHelper : TextAreaTagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string RequiredAttributeName = "asp-required";
        private const string DisabledAttributeName = "asp-disabled";

        /// <summary>
        /// Trạng thái disable của control
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public string IsDisabled { set; get; }

        /// <summary>
        /// Bắt buộc nhập
        /// </summary>
        [HtmlAttributeName(RequiredAttributeName)]
        public bool IsRequired { set; get; }

        public CmsTextAreaTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "textarea";
            output.TagMode = TagMode.StartTagAndEndTag;

            string classValue = output.Attributes.ContainsName("class") ? $"{output.Attributes["class"].Value} form-control" : "form-control";
            output.Attributes.SetAttribute("class", classValue);

            bool.TryParse(IsDisabled, out bool disabled);
            if (disabled)
            {
                TagHelperAttribute d = new TagHelperAttribute("disabled", "disabled");
                output.Attributes.Add(d);
            }

            object rowsNumber = output.Attributes.ContainsName("rows") ? output.Attributes["rows"].Value : 4;
            output.Attributes.SetAttribute("rows", rowsNumber);
            object colsNumber = output.Attributes.ContainsName("cols") ? output.Attributes["cols"].Value : 20;
            output.Attributes.SetAttribute("cols", colsNumber);

            if (IsRequired)
            {
                output.PreElement.SetHtmlContent("<div class='input-group input-group-required'>");
                output.PostElement.SetHtmlContent("<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>");
            }

            base.Process(context, output);
        }
    }
}