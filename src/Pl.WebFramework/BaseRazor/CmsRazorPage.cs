using Microsoft.AspNetCore.Mvc.Razor;
using Pl.Core;
using System.Security.Claims;
using Pl.WebFramework.TagHelpers;

namespace Pl.WebFramework.BaseRazor
{
    public abstract class CmsRazorPage<T> : RazorPage<T>
    {
        private string currentUrl;

        /// <summary>
        /// Id người dung đang đăng nhập vào trong hệ thống hiện tại
        /// </summary>
        protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// Lấy url hiện tại của hệ thống
        /// </summary>
        /// <returns>string</returns>
        public string ThisPageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(currentUrl))
                {
                    currentUrl = Context.GetThisPageUrl();
                }
                return currentUrl;
            }
        }

        /// <summary>
        /// Gets a selected tab name (used in admin area to store selected tab name)
        /// </summary>
        /// <param name="dataKeyPrefix">Key prefix. Pass null to ignore</param>
        /// <returns>Name</returns>
        public string GetSelectedTabName(string dataKeyPrefix = null)
        {
            //chú ý đồng bộ với hàm "SaveSelectedTabName" ở file Pl.WebFramework.BaseController.CmsController.cs
            string tabName = string.Empty;
            string dataKey = TabsTagHelper.TagDataKey;
            if (!string.IsNullOrEmpty(dataKeyPrefix))
            {
                dataKey += $"-{dataKeyPrefix}";
            }

            if (ViewData.ContainsKey(dataKey))
            {
                tabName = ViewData[dataKey].ToString();
            }

            if (ViewContext.TempData.ContainsKey(dataKey))
            {
                tabName = ViewContext.TempData[dataKey].ToString();
            }

            return tabName;
        }

    }
}
