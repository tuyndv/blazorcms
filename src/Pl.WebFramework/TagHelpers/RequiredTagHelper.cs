using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Pl.WebFramework.TagHelpers
{
    [HtmlTargetElement("required", TagStructure = TagStructure.WithoutEndTag)]
    public class RequiredTagHelper : TagHelper
    {
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

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "required");
            output.Content.SetContent("*");
        }
    }
}