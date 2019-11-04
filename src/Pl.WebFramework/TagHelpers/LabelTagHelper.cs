using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Pl.WebFramework.TagHelpers
{
    [HtmlTargetElement("label", Attributes = ForAttributeName)]
    public class LabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        private readonly IOptions<MvcViewOptions> optionsAccessor;
        private readonly IStringLocalizer<LabelTagHelper> _localizer;

        public LabelTagHelper(IOptions<MvcViewOptions> _optionsAccessor, IStringLocalizer<LabelTagHelper> localizer)
        {
            optionsAccessor = _optionsAccessor;
            _localizer = localizer;
        }

        public override int Order => -1001;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("required")]
        public bool? Required { get; set; }

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
            bool isRequired = Required ?? For.Metadata.AdditionalValues.TryGetValue("PLRequired", out _);
            if (isRequired)
            {
                tagBuilder.MergeAttribute("class", "label-required");
                tagBuilder.MergeAttribute("title", _localizer["Yêu cầu nhập."]);
            }

            string fullName = NameAndIdProvider.GetFullHtmlFieldName(ViewContext, For.Name);
            string idString = NameAndIdProvider.CreateSanitizedId(ViewContext, fullName, optionsAccessor.Value.HtmlHelperOptions.IdAttributeDotReplacement);
            tagBuilder.Attributes.Add("for", idString);

            string labelText = For.ModelExplorer.Metadata.DisplayName ?? For.Name ?? For.ModelExplorer.Metadata.PropertyName;
            tagBuilder.InnerHtml.SetContent(labelText);

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