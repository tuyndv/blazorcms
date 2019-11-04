using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace Pl.WebFramework.TagHelpers
{
    [HtmlTargetElement("labeltext", Attributes = ForAttributeName)]
    public class LabelTextTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-text";
        private readonly IStringLocalizer<LabelTagHelper> _localizer;

        public LabelTextTagHelper(IStringLocalizer<LabelTagHelper> localizer)
        {
            DisplayHint = "";
            Required = false;
            _localizer = localizer;
        }

        public override int Order => -1003;

        [HtmlAttributeName(ForAttributeName)]
        public string For { get; set; }

        [HtmlAttributeName("required")]
        public bool Required { get; set; }

        [HtmlAttributeName("hint")]
        public string DisplayHint { get; set; }

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

            if (For == null)
            {
                throw new ArgumentNullException(nameof(For));
            }

            output.TagName = "label";
            TagBuilder tagBuilder = new TagBuilder("label");
            if (Required)
            {
                tagBuilder.MergeAttribute("class", "label-required");
                tagBuilder.MergeAttribute("title", _localizer["Yêu cầu nhập."]);
            }

            tagBuilder.InnerHtml.SetContent(For);

            if (!string.IsNullOrEmpty(DisplayHint))
            {
                TagBuilder builder = new TagBuilder("i");
                builder.MergeAttribute("class", "fa fa-question-circle info hellp-hit");
                builder.MergeAttribute("data-toggle", "tooltip");
                builder.MergeAttribute("data-placement", "top");
                builder.MergeAttribute("title", DisplayHint);
                tagBuilder.InnerHtml.AppendHtml(builder);
            }

            output.MergeAttributes(tagBuilder);
            if (!output.IsContentModified)
            {
                TagHelperContent childContent = await output.GetChildContentAsync();
                if (childContent.IsEmptyOrWhiteSpace)
                {
                    if (tagBuilder.HasInnerHtml)
                    {
                        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
                    }
                }
                else
                {
                    output.Content.SetHtmlContent(childContent);
                }
            }
        }
    }
}