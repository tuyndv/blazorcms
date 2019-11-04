using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Pl.Core;
using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using Pl.Core.Security;
using Pl.WebFramework.FilterAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using Pl.WebFramework.TagHelpers;

namespace Pl.WebFramework.BaseController
{
    [AntiForgery]
    [Authorize(Roles = PermissionConstants.CmsDashbroad)]
    public class CmsController : Controller
    {

        /// <summary>
        /// Ghi nhật ký người dùng
        /// </summary>
        protected readonly IActivityLogData _activityLogData;

        protected readonly IStringLocalizer<CmsController> _localizer;

        protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public CmsController(
            IStringLocalizer<CmsController> localizer,
            IActivityLogData activityLogData)
        {
            _localizer = localizer;
            _activityLogData = activityLogData;
        }

        /// <summary>
        /// Ghi lại trạng thái chọn tab
        /// Gồm tab mặc định và tab động
        /// </summary>
        /// <param name="tabName">Tên tab cần lấy lưu. Nếu để trống thì hệ thống tự lấy trền from về</param>
        /// <param name="persistForTheNextRequest">Lưu trạng thái tab cho lần yêu cầu sau hay không</param>
        protected virtual void SaveSelectedTabName(string tabName = "", bool persistForTheNextRequest = true)
        {
            SaveSelectedTabName(tabName, "selected-tab-name", null, persistForTheNextRequest);
            if (!Request.Method.Equals(WebRequestMethods.Http.Post, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("selected-tab-name-", StringComparison.InvariantCultureIgnoreCase))
                {
                    SaveSelectedTabName(null, key, key.Substring("selected-tab-name-".Length), persistForTheNextRequest);
                }
            }
        }

        /// <summary>
        /// Ghi lại trạng thái chọn tab
        /// </summary>
        /// <param name="tabName">Tên tab cần lấy lưu. Nếu để trống thì hệ thống tự lấy trền from về</param>
        /// <param name="formKey">tên hiden lưu thông tin active tab</param>
        /// <param name="dataKeyPrefix">Tiền tố cho các tab con</param>
        /// <param name="persistForTheNextRequest">Lưu trạng thái tab cho lần yêu cầu sau hay không</param>
        protected virtual void SaveSelectedTabName(string tabName, string formKey, string dataKeyPrefix, bool persistForTheNextRequest)
        {
            //Chú ý. hàm này đồng bộ với hàm "GetSelectedTabName" ở file Pl.WebFramework.Extensions.cs
            if (string.IsNullOrEmpty(tabName))
            {
                tabName = Request.Form[formKey];
            }

            if (string.IsNullOrEmpty(tabName))
            {
                return;
            }

            string dataKey = TabsTagHelper.TagDataKey;
            if (!string.IsNullOrEmpty(dataKeyPrefix))
            {
                dataKey += $"-{dataKeyPrefix}";
            }

            if (persistForTheNextRequest)
            {
                TempData[dataKey] = tabName;
            }
            else
            {
                ViewData[dataKey] = tabName;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.IsAjaxRequest())
            {
                ModelState.Clear();
            }

            if (!ModelState.IsValid)
            {
                foreach (KeyValuePair<string, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry> item in ModelState)
                {
                    if (item.Value.Errors?.Count > 0)
                    {
                        foreach (Microsoft.AspNetCore.Mvc.ModelBinding.ModelError error in item.Value.Errors)
                        {
                            ErrorNotification(error.ErrorMessage);
                        }
                    }
                }
            }

            base.OnActionExecuted(context);
        }

        #region Notification Helper

        /// <summary>
        /// Hàm tạo log khi xử lý một hành động nào đó trong hệ thống
        /// </summary>
        /// <param name="message">Nội dung</param>
        /// <param name="userId">Id người dùng</param>
        /// <param name="complete">Trạng thái thành công</param>
        /// <param name="activityLogType">Id hành động</param>
        /// <param name="objectType">Id loại đối tượng</param>
        /// <param name="objectJson">Chuỗi Json đối tượng</param>
        /// <param name="isShowNotifi">Có hiển thị notification hay không</param>
        /// <param name="objectId">Id đối tượng nếu có, mặc định bằng 0</param>
        /// <returns>string</returns>
        protected virtual async Task<string> SaveActivityLogAsync(string message, string userId = null, bool complete = true, ActivityTypeEnum activityLogType = ActivityTypeEnum.System, ObjectTypeEnum objectType = ObjectTypeEnum.System, object objectJson = null, bool isShowNotifi = true, string objectId = null)
        {
            GuardClausesParameter.Null(activityLogType, nameof(activityLogType));
            GuardClausesParameter.Null(objectType, nameof(objectType));

            string notifiString = $"{_localizer[CoreConstants.ActivityTypes[activityLogType]]} {_localizer[CoreConstants.ObjectTypes[objectType]]} \"{message}\" {_localizer[complete ? "thành công" : "không thành công"]}.";
            if (isShowNotifi)
            {
                if (complete)
                {
                    SuccessNotification(notifiString);
                }
                else
                {
                    ErrorNotification(notifiString);
                }
            }
            var activityLog = new ActivityLog()
            {
                Complete = complete,
                IpAddress = HttpContext.GetCurrentIpAddress(),
                Message = message,
                UserId = userId ?? CurrentUserId,
                ObjectId = objectId,
                ObjectJson = JsonSerializer.Serialize(objectJson, new JsonSerializerOptions() { WriteIndented = true }),
                ObjectType = objectType,
                PageUrl = HttpContext.GetThisPageUrl(),
                Type = activityLogType
            };
            await _activityLogData.InsertAsync(activityLog);
            return notifiString;
        }

        /// <summary>
        /// Hiện thị thông báo cho trang
        /// </summary>
        /// <param name="message">Thông báo</param>
        /// <param name="persistForTheNextRequest">Có hiển thị ở lần request tiếp theo hay không</param>
        protected virtual void InfoNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Info, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Hiện thị thống báo thành công
        /// </summary>
        /// <param name="message">Thông báo</param>
        /// <param name="persistForTheNextRequest">Thông báo sẽ được hiện thì cho lần yêu cầu sau hay không</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        /// <param name="message">Thông báo</param>
        /// <param name="persistForTheNextRequest">Thông báo sẽ được hiện thì cho lần yêu cầu sau hay không</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        /// <param name="exception">Lỗi</param>
        /// <param name="persistForTheNextRequest">Thông báo sẽ được hiện thì cho lần yêu cầu sau hay không</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }

        /// <summary>
        /// Thêm hiện thị nhiều thông báo
        /// </summary>
        /// <param name="type">Loại thông báo</param>
        /// <param name="message">Thông báo</param>
        /// <param name="persistForTheNextRequest">Thông báo sẽ được hiện thì cho lần yêu cầu sau hay không</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("pl.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                {
                    TempData[dataKey] = new List<string>();
                }
                else
                {
                    if (TempData[dataKey] is string[])
                    {
                        TempData[dataKey] = ((string[])TempData[dataKey]).ToList();
                    }
                }
                ((IList<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                {
                    ViewData[dataKey] = new List<string>();
                } ((IList<string>)ViewData[dataKey]).Add(message);
            }
        }

        /// <summary>
        /// Hàm gỡ bỏ thông báo lỗi
        /// </summary>
        /// <param name="type">Loại lỗi</param>
        protected virtual void RemoveNotification(NotifyType type)
        {
            string dataKey = string.Format("pl.notifications.{0}", type);
            if (TempData[dataKey] != null)
            {
                TempData.Remove(dataKey);
            }

            if (ViewData[dataKey] != null)
            {
                ViewData.Remove(dataKey);
            }
        }

        #endregion Notification Helper
    }

    /// <summary>
    /// Định nghĩa các loại notification trong cms
    /// </summary>
    public enum NotifyType
    {
        Success,
        Error,
        Info
    }
}