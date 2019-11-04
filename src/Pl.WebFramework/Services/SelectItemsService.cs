using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Pl.Core;
using Pl.Core.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pl.WebFramework
{
    public class SelectItemsService
    {
        private readonly IStringLocalizer<SelectItemsService> _localizer;

        public SelectItemsService(IStringLocalizer<SelectItemsService> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Lấy người dùng hiện tại hoặc lấy tất cả
        /// Hàm này chỉ gọi được khi đã có người login
        /// </summary>
        /// <param name="claimsPrincipal">User claims</param>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> GetAllUserOrOnlyMeSelectedItems(ClaimsPrincipal claimsPrincipal)
        {
            GuardClausesParameter.Null(claimsPrincipal, nameof(claimsPrincipal));

            string userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            return new List<SelectListItem>() {
                new SelectListItem() { Text = _localizer["Tất cả mọi người"], Value = "" },
                new SelectListItem() { Text = _localizer["Chỉ mình tôi"], Value = userId, Selected = true  },
            };
        }

        /// <summary>
        /// Lấy danh sách lựa chọn giá trị Boolean và null
        /// </summary>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> GetBooleanSelectedItems()
        {
            return new List<SelectListItem>
            {
                new SelectListItem() { Text = _localizer["Tất cả"], Value = "", Selected = true },
                new SelectListItem() { Text = _localizer["Đúng"], Value = "True" },
                new SelectListItem() { Text = _localizer["Sai"], Value = "False" }
            };
        }

        /// <summary>
        /// Lấy danh sách loại video trong hệ thống
        /// </summary>
        /// <param name="withSelectAll">Có kèm lựa chọn tất hay không</param>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> GetResourceFileTypeSelectedItems(bool withSelectAll = false)
        {
            List<SelectListItem> query = CoreConstants.ResourceTypes.Select(q => new SelectListItem()
            {
                Text = _localizer[q.Value],
                Value = ((int)q.Key).ToString(),
            }).ToList();
            if (withSelectAll)
            {
                query.Insert(0, new SelectListItem() { Text = _localizer["Tất cả"], Value = "", Selected = true });
            }

            return query;
        }

        /// <summary>
        /// Lấy danh sách các leve lỗi hệ thống
        /// </summary>
        /// <param name="withSelectAll">Có kèm lựa chọn tất hay không</param>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> GetErrorLogLevelSelectItems(bool withSelectAll = true)
        {
            List<SelectListItem> query = CoreConstants.ErrorLogLevels.Select(q => new SelectListItem()
            {
                Text = _localizer[q.Value],
                Value = ((int)q.Key).ToString(),
            }).ToList();
            if (withSelectAll)
            {
                query.Insert(0, new SelectListItem() { Text = _localizer["Tất cả"], Value = "", Selected = true });
            }

            return query;
        }

        /// <summary>
        /// Lấy danh sách các giới tính trong hệ thống
        /// </summary>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> GetGenderSelectListItem()
        {
            return CoreConstants.Genders.Select(q => new SelectListItem()
            {
                Text = _localizer[q.Value],
                Value = ((int)q.Key).ToString()
            }).ToList();
        }

        /// <summary>
        /// Lấy danh sách cách mở link cho phần menu
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> GetSelectLinkOpenType()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = _localizer["Tải lại trang"], Value = "" },
                new SelectListItem() { Text = _localizer["Mở tab mới"], Value = "_blank" },
                new SelectListItem() { Text = _localizer["Tải lại cửa số chính nếu link được click ở cửa popup"], Value = "_parent" },
                new SelectListItem() { Text = _localizer["Tải lại cửa sổ chính nều link được đặt ở iframe"], Value = "_top" }
            };
        }
    }
}
