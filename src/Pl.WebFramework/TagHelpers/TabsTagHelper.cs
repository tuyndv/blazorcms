using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;
using Pl.Core;
using Pl.WebFramework;
using Pl.WebFramework.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pl.WebFramework.TagHelpers
{
    [HtmlTargetElement("tabs", Attributes = IdAttributeName)]
    public class TabsTagHelper : TagHelper
    {
        public static readonly string TagDataKey = "pl.selected-tab-name";
        private const string IdAttributeName = "id";
        private const string TabNameToSelectAttributeName = "asp-tab-select";
        private const string TabNamePerfixAttributeName = "asp-tab-perfix";
        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Tên tab được lựa chọn
        /// </summary>
        [HtmlAttributeName(TabNameToSelectAttributeName)]
        public string TabNameToSelect { set; get; }

        /// <summary>
        /// Mã cộng thêm cho tag động
        /// </summary>
        [HtmlAttributeName(TabNamePerfixAttributeName)]
        public string TabNamePerfix { set; get; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public TabsTagHelper(IHtmlHelper htmlHelper)
        {
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

            List<TabContextItem> tabContext = new List<TabContextItem>();
            context.Items.Add(typeof(TabsTagHelper), tabContext);
            StringValues tabNameToSelect = TabNameToSelect;
            if (string.IsNullOrEmpty(tabNameToSelect))
            {
                tabNameToSelect = ViewContext.HttpContext.Request.Query["tabNameToSelect"];
            }

            if (string.IsNullOrEmpty(tabNameToSelect))
            {
                tabNameToSelect = _htmlHelper.GetSelectedTabName();
            }

            if (!string.IsNullOrEmpty(tabNameToSelect))
            {
                context.Items.Add("tabNameToSelect", tabNameToSelect);
            }

            await output.GetChildContentAsync();

            TagBuilder tabsTitle = new TagBuilder("ul");
            tabsTitle.AddCssClass("nav");
            tabsTitle.AddCssClass("nav-tabs");

            TagBuilder tabsContent = new TagBuilder("div");
            tabsContent.AddCssClass("tab-content");

            foreach (TabContextItem tabItem in tabContext)
            {
                tabsTitle.InnerHtml.AppendHtml(tabItem.Title);
                tabsContent.InnerHtml.AppendHtml(tabItem.Content);
            }

            output.Content.AppendHtml(tabsTitle.ToHtmlString());
            output.Content.AppendHtml(tabsContent.ToHtmlString());

            string hiddenName = "selected-tab-name";
            if (!string.IsNullOrEmpty(TabNamePerfix))
            {
                hiddenName += $"-{TabNamePerfix}";
            }

            TagBuilder selectedTabInput = new TagBuilder("input");
            selectedTabInput.Attributes.Add("type", "hidden");
            selectedTabInput.Attributes.Add("id", hiddenName);
            selectedTabInput.Attributes.Add("name", hiddenName);
            selectedTabInput.Attributes.Add("value", tabNameToSelect);
            output.PreContent.SetHtmlContent(selectedTabInput.ToHtmlString());

            if (output.Attributes.ContainsName("id"))
            {
                TagBuilder script = new TagBuilder("script");
                script.InnerHtml.AppendHtml("$(document).ready(function () {bindBootstrapTabSelectEvent('" + output.Attributes["id"].Value + "', '" + hiddenName + "');});");
                output.PostContent.SetHtmlContent(script.ToHtmlString());
            }

            output.TagName = "div";
            const string itemClass = "nav-tabs-custom";
            string classValue = output.Attributes.ContainsName("class") ? $"{output.Attributes["class"].Value} {itemClass}" : itemClass;
            output.Attributes.SetAttribute("class", classValue);
        }
    }

    [HtmlTargetElement("tab", ParentTag = "pltabs", Attributes = NameAttributeName)]
    public class TabTagHelper : TagHelper
    {
        private const string NameAttributeName = "asp-name";
        private const string TitleAttributeName = "asp-title";
        private const string DefaultAttributeName = "asp-default";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// Tiêu đề của tab
        /// </summary>
        [HtmlAttributeName(TitleAttributeName)]
        public string Title { set; get; }

        /// <summary>
        /// Tab mặc định
        /// </summary>
        [HtmlAttributeName(DefaultAttributeName)]
        public bool IsDefault { set; get; }

        /// <summary>
        /// Tên tab
        /// </summary>
        [HtmlAttributeName(NameAttributeName)]
        public string Name { set; get; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public TabTagHelper(IHtmlHelper htmlHelper)
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

            IViewContextAware viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            string tabNameToSelect = context.Items.ContainsKey("tabNameToSelect") ? context.Items["tabNameToSelect"].ToString() : "";

            if (string.IsNullOrEmpty(tabNameToSelect))
            {
                tabNameToSelect = _htmlHelper.GetSelectedTabName();
            }

            if (string.IsNullOrEmpty(tabNameToSelect) && IsDefault)
            {
                tabNameToSelect = Name;
            }

            TagBuilder tabTitle = new TagBuilder("li");
            TagBuilder a = new TagBuilder("a")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("data-tab-name", Name),
                    new KeyValuePair<string, string>("href", $"#{Name}"),
                    new KeyValuePair<string, string>("data-toggle", "tab"),
                }
            };
            a.InnerHtml.AppendHtml(Title);

            if (context.AllAttributes.ContainsName("class"))
            {
                tabTitle.Attributes.Add("class", context.AllAttributes["class"].Value.ToString());
            }

            tabTitle.InnerHtml.AppendHtml(a.ToHtmlString());

            TagBuilder tabContent = new TagBuilder("div");
            tabContent.AddCssClass("tab-pane");
            tabContent.Attributes.Add("id", Name);
            tabContent.InnerHtml.AppendHtml(output.GetChildContentAsync().Result.GetContent());
            if (tabNameToSelect == Name)
            {
                tabTitle.AddCssClass("active");
                tabContent.AddCssClass("active");
            }

            List<TabContextItem> tabContext = (List<TabContextItem>)context.Items[typeof(TabsTagHelper)];
            tabContext.Add(new TabContextItem()
            {
                Title = tabTitle.ToHtmlString(),
                Content = tabContent.ToHtmlString(),
                IsDefault = IsDefault
            });

            output.SuppressOutput();
        }
    }
}