using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Pl.Core;
using System;
using System.Collections.Generic;

namespace Pl.WebFramework.TagHelpers
{
    [HtmlTargetElement("plselect", TagStructure = TagStructure.WithoutEndTag)]
    public class SelectTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string NameAttributeName = "asp-for-name";
        private const string ItemsAttributeName = "asp-items";
        private const string DisabledAttributeName = "asp-multiple";
        private const string RequiredAttributeName = "asp-required";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Chỉ dõ model cần edit
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Tên của control
        /// </summary>
        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        /// <summary>
        /// Danh sách item
        /// </summary>
        [HtmlAttributeName(ItemsAttributeName)]
        public IEnumerable<SelectListItem> Items { set; get; } = new List<SelectListItem>();

        /// <summary>
        /// Yêu cầu bắt buộc chọn hay không
        /// </summary>
        [HtmlAttributeName(RequiredAttributeName)]
        public bool IsRequired { set; get; } = false;

        [HtmlAttributeName(DisabledAttributeName)]
        public bool IsMultiple { set; get; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public SelectTagHelper(IHtmlHelper htmlHelper)
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

            if (IsRequired)
            {
                output.PreElement.SetHtmlContent("<div class='input-group input-group-required'>");
                output.PostElement.SetHtmlContent("<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>");
            }

            IViewContextAware viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            ReadOnlyTagHelperAttributeList attributes = context.AllAttributes;
            foreach (TagHelperAttribute attribute in attributes)
            {
                if (!attribute.Name.Equals(ForAttributeName)
                    && !attribute.Name.Equals(NameAttributeName)
                    && !attribute.Name.Equals(ItemsAttributeName)
                    && !attribute.Name.Equals(DisabledAttributeName)
                    && !attribute.Name.Equals(RequiredAttributeName))
                {
                    htmlAttributes.Add(attribute.Name, attribute.Value);
                }
            }

            string tagName = For != null ? For.Name : Name;
            if (!string.IsNullOrEmpty(tagName))
            {
                IHtmlContent selectList;
                if (IsMultiple)
                {
                    selectList = _htmlHelper.Editor(tagName, "MultiSelect", new { htmlAttributes, SelectList = Items });
                }
                else
                {
                    if (htmlAttributes.ContainsKey("class"))
                    {
                        htmlAttributes["class"] += " form-control";
                    }
                    else
                    {
                        htmlAttributes.Add("class", "form-control");
                    }

                    selectList = _htmlHelper.DropDownList(tagName, Items, htmlAttributes);
                }
                output.Content.SetHtmlContent(selectList.ToHtmlString());
            }
        }
    }
}