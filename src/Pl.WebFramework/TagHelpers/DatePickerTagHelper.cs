using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Pl.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Pl.WebFramework.TagHelpers
{
    /// <summary>
    /// nop-date-picker tag helper
    /// </summary>
    [HtmlTargetElement("datepicker", Attributes = DayNameAttributeName + "," + MonthNameAttributeName + "," + YearNameAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class DatePickerTagHelper : TagHelper
    {
        private const string DayNameAttributeName = "asp-day-name";
        private const string MonthNameAttributeName = "asp-month-name";
        private const string YearNameAttributeName = "asp-year-name";
        private const string BeginYearAttributeName = "asp-begin-year";
        private const string EndYearAttributeName = "asp-end-year";
        private const string SelectedDayAttributeName = "asp-selected-day";
        private const string SelectedMonthAttributeName = "asp-selected-month";
        private const string SelectedYearAttributeName = "asp-selected-year";
        private const string WrapTagsAttributeName = "asp-wrap-tags";
        private readonly IHtmlHelper _htmlHelper;

        protected IHtmlGenerator Generator { get; set; }

        /// <summary>
        /// Tên control chỉnh ngày
        /// </summary>
        [HtmlAttributeName(DayNameAttributeName)]
        public string DayName { get; set; }

        /// <summary>
        /// Tên control chỉnh tháng
        /// </summary>
        [HtmlAttributeName(MonthNameAttributeName)]
        public string MonthName { get; set; }

        /// <summary>
        /// Tên control chỉnh năm
        /// </summary>
        [HtmlAttributeName(YearNameAttributeName)]
        public string YearName { get; set; }

        /// <summary>
        /// Năm bắt đầu, có thể bỏ trống đề lấy default cách năm hiện tại 10 năm
        /// </summary>
        [HtmlAttributeName(BeginYearAttributeName)]
        public int? BeginYear { get; set; }

        /// <summary>
        /// Năm kết thúc, có thể bỏ trống để lấy default là năm hiện tại
        /// </summary>
        [HtmlAttributeName(EndYearAttributeName)]
        public int? EndYear { get; set; }

        /// <summary>
        /// Giá trị lựa chọn ngày hiện tại
        /// </summary>
        [HtmlAttributeName(SelectedDayAttributeName)]
        public int? SelectedDay { get; set; }

        /// <summary>
        /// Giá trị lựa chọn tháng hiện tại
        /// </summary>
        [HtmlAttributeName(SelectedMonthAttributeName)]
        public int? SelectedMonth { get; set; }

        /// <summary>
        /// Giá trị lựa chọn năm hiện tại
        /// </summary>
        [HtmlAttributeName(SelectedYearAttributeName)]
        public int? SelectedYear { get; set; }

        /// <summary>
        /// Thẻ Wrap
        /// </summary>
        [HtmlAttributeName(WrapTagsAttributeName)]
        public string WrapTags { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public DatePickerTagHelper(IHtmlGenerator generator, IHtmlHelper htmlHelper)
        {
            Generator = generator;
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

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "date-picker-wrapper");

            TagBuilder daysList = new TagBuilder("select");
            TagBuilder monthsList = new TagBuilder("select");
            TagBuilder yearsList = new TagBuilder("select");

            daysList.Attributes.Add("name", DayName);
            monthsList.Attributes.Add("name", MonthName);
            yearsList.Attributes.Add("name", YearName);

            List<string> tagHelperAttributes = new List<string>
            {
                DayNameAttributeName,
                MonthNameAttributeName,
                YearNameAttributeName,
                BeginYearAttributeName,
                EndYearAttributeName,
                SelectedDayAttributeName,
                SelectedMonthAttributeName,
                SelectedYearAttributeName,
                WrapTagsAttributeName
            };
            Dictionary<string, object> customerAttributes = new Dictionary<string, object>();
            foreach (TagHelperAttribute attribute in context.AllAttributes)
            {
                if (!tagHelperAttributes.Contains(attribute.Name))
                {
                    customerAttributes.Add(attribute.Name, attribute.Value);
                }
            }
            IDictionary<string, object> htmlAttributesDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(customerAttributes);
            daysList.MergeAttributes(htmlAttributesDictionary, true);
            monthsList.MergeAttributes(htmlAttributesDictionary, true);
            yearsList.MergeAttributes(htmlAttributesDictionary, true);

            StringBuilder days = new StringBuilder();
            StringBuilder months = new StringBuilder();
            StringBuilder years = new StringBuilder();

            days.AppendFormat("<option value='{0}'>{1}</option>", "0", "Chọn ngày");
            for (int i = 1; i <= 31; i++)
            {
                days.AppendFormat("<option value='{0}'{1}>{0}</option>", i, (SelectedDay.HasValue && SelectedDay.Value == i) ? " selected=\"selected\"" : null);
            }

            months.AppendFormat("<option value='{0}'>{1}</option>", "0", "Chọn tháng");
            for (int i = 1; i <= 12; i++)
            {
                months.AppendFormat("<option value='{0}'{1}>{2}</option>", i, (SelectedMonth.HasValue && SelectedMonth.Value == i) ? " selected=\"selected\"" : null, CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i));
            }

            years.AppendFormat("<option value='{0}'>{1}</option>", "0", "Chọn năm");

            if (BeginYear == null)
            {
                BeginYear = DateTime.UtcNow.Year - 10;
            }

            if (EndYear == null)
            {
                EndYear = DateTime.UtcNow.Year;
            }

            if (EndYear > BeginYear)
            {
                for (int i = BeginYear.Value; i <= EndYear.Value; i++)
                {
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i, (SelectedYear.HasValue && SelectedYear.Value == i) ? " selected=\"selected\"" : null);
                }
            }
            else
            {
                for (int i = BeginYear.Value; i >= EndYear.Value; i--)
                {
                    years.AppendFormat("<option value='{0}'{1}>{0}</option>", i, (SelectedYear.HasValue && SelectedYear.Value == i) ? " selected=\"selected\"" : null);
                }
            }

            daysList.InnerHtml.AppendHtml(days.ToString());
            monthsList.InnerHtml.AppendHtml(months.ToString());
            yearsList.InnerHtml.AppendHtml(years.ToString());

            if (bool.TryParse(WrapTags, out bool wrapTags) && wrapTags)
            {
                string wrapDaysList = "<span class=\"days-list select-wrapper\">" + daysList.ToHtmlString() + "</span>";
                string wrapMonthsList = "<span class=\"months-list select-wrapper\">" + monthsList.ToHtmlString() + "</span>";
                string wrapYearsList = "<span class=\"years-list select-wrapper\">" + yearsList.ToHtmlString() + "</span>";

                output.Content.AppendHtml(wrapDaysList);
                output.Content.AppendHtml(wrapMonthsList);
                output.Content.AppendHtml(wrapYearsList);
            }
            else
            {
                output.Content.AppendHtml(daysList);
                output.Content.AppendHtml(monthsList);
                output.Content.AppendHtml(yearsList);
            }
        }
    }
}