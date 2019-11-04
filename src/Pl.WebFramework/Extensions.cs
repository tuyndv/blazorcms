using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pl.Core;
using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using Pl.WebFramework.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace Pl.WebFramework
{
    public static class Extensions
    {

        /// <summary>
        /// Hàm tạo icon hướng dẫn
        /// </summary>
        /// <param name="helper">HTML helper</param>
        /// <param name="value">Nội dung hướng dẫn</param>
        /// <returns>IHtmlContent</returns>
        public static IHtmlContent Hint(this IHtmlHelper helper, string value)
        {
            GuardClausesParameter.Null(helper, nameof(helper));

            //Hàm này đồng bộ với file QiLabelTagHelper
            TagBuilder builder = new TagBuilder("i");
            builder.MergeAttribute("title", value);
            builder.MergeAttribute("class", "fa fa-question-circle info hellp-hit");
            builder.MergeAttribute("data-toggle", "tooltip");
            builder.MergeAttribute("data-placement", "top");
            return new HtmlString(builder.ToHtmlString());
        }

        /// <summary>
        /// Hàm hỗ chợ build ids sang dạng string
        /// </summary>
        /// <param name="ids">Danh sách Id</param>
        /// <returns>string</returns>
        public static string GetStringIds(this List<long> ids)
        {
            return string.Format("Ids => {0}", string.Join(",", ids));
        }

        /// <summary>
        /// Chuyển một chuỗi string thành một chuỗi html string
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>HtmlString</returns>
        public static HtmlString ToHtmlString(this string text)
        {
            return new HtmlString(text);
        }

        /// <summary>
        /// Lấy danh sách ngôn ngữ và .net hỗ trợ
        /// </summary>
        /// <param name="helper">Html helper</param>
        /// <returns>List SelectListItem</returns>
        public static List<SelectListItem> GetSpecificCulturesSelectItem(this IHtmlHelper helper)
        {
            GuardClausesParameter.Null(helper, nameof(helper));

            return CultureInfo.GetCultures(CultureTypes.SpecificCultures).OrderBy(x => x.EnglishName).Select(x => new SelectListItem()
            {
                Value = x.IetfLanguageTag,
                Text = string.Format("{0}. {1}", x.EnglishName, x.IetfLanguageTag)
            }).ToList();
        }

        /// <summary>
        /// Hàm chuyển từ một list TreeItem<T> sang một danh sách SelectListItem
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng</typeparam>
        /// <param name="treeItems">Cây đối tượng</param>
        /// <param name="selectListItems">Danh sách SelectListItem</param>
        /// <param name="textGeter">Hàm lấy giá trị text từ đối tượng</param>
        /// <param name="idGeter">Hàm lấy giá trị id từ đối tượng</param>
        /// <param name="treeName">Tên tree</param>
        /// <param name="parentToChildString">Ký tự thể hiện liên kết cha con</param>
        /// <returns>List SelectListItem</returns>
        public static List<SelectListItem> BuildTreeSelectListItem<T>(this IEnumerable<TreeItem<T>> treeItems, List<SelectListItem> selectListItems, Func<T, string> textGeter, Func<T, string> idGeter, string treeName = "", string parentToChildString = " > ")
        {
            foreach (var item in treeItems)
            {
                var text = textGeter(item.Item);
                var currentName = string.IsNullOrEmpty(treeName) ? text : $"{treeName}{parentToChildString}{text}";
                selectListItems.Add(new SelectListItem()
                {
                    Text = currentName,
                    Value = idGeter(item.Item)
                });
                selectListItems = BuildTreeSelectListItem(item.Children, selectListItems, textGeter, idGeter, currentName);
            }
            return selectListItems;
        }

        /// <summary>
        /// Lấy trang hiện tại của yêu cầu.
        /// </summary>
        /// <param name="command">DataSourceRequest</param>
        /// <returns>long</returns>
        public static long CurrentPage(this DataSourceRequest command)
        {
            if (command.Take <= 0)
            {
                return 1;
            }

            return (command.Skip / command.Take) + 1;
        }

        /// <summary>
        /// Lấy trang hiện tại của yêu cầu.
        /// </summary>
        /// <param name="data">DataSourceRequest</param>
        /// <returns>string</returns>
        public static string ToJson<T>(this IDataSourceResult<T> data)
        {
            if (data != null)
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets a selected tab name (used in admin area to store selected tab name)
        /// </summary>
        /// <param name="helper">Html helper</param>
        /// <param name="dataKeyPrefix">Key prefix. Pass null to ignore</param>
        /// <returns>Name</returns>
        public static string GetSelectedTabName(this IHtmlHelper helper, string dataKeyPrefix = null)
        {
            //chú ý đồng bộ với hàm "SaveSelectedTabName" ở file Pl.WebFramework.BaseController.CmsController.cs
            string tabName = string.Empty;
            string dataKey = TabsTagHelper.TagDataKey;
            if (!string.IsNullOrEmpty(dataKeyPrefix))
            {
                dataKey += $"-{dataKeyPrefix}";
            }

            if (helper.ViewData.ContainsKey(dataKey))
            {
                tabName = helper.ViewData[dataKey].ToString();
            }

            if (helper.ViewContext.TempData.ContainsKey(dataKey))
            {
                tabName = helper.ViewContext.TempData[dataKey].ToString();
            }

            return tabName;
        }

    }
}