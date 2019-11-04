using Microsoft.AspNetCore.Identity;
using Pl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Pl.Core.Entities
{

    public class User : IdentityUser, IBaseEntity
    {

        /// <summary>
        /// Ảnh Avatar
        /// ex 2017/03/27/avatar.jpg
        /// </summary>
        public string AvatarImage { get; set; }

        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Giới thiệu về bản thân
        /// </summary>
        public string AboutMe { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// Cơ quan đang làm việc
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Ghi chú quản trị
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Địa trỉ ip ở lần hoạt động cuối cùng
        /// </summary>
        [StringLength(200)]
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Thời gian đăng nhập lần cuối cùng
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// Chuỗi phân quyền
        /// </summary>
        public string RolesString { get; set; }

        /// <summary>
        /// Lấy mảng quyền
        /// </summary>
        public List<string> Roles => RolesString?.Split(",").ToList() ?? new List<string>();

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool Active { get; set; } = false;

        /// <summary>
        /// Tài khoản đã bị xóa
        /// </summary>
        public bool Deleted { get; set; } = false;

        /// <summary>
        /// Tiền tệ mà người dùng sử dụng dạng, USD, VND ...
        /// </summary>
        public string CurrencyCode { get; set; } = "VND";

        /// <summary>
        /// Ngôn ngữ mà người dùng sử dụng dạng, en-US, vi-VN ...
        /// </summary>
        public string LanguageCulture { get; set; } = "vi-VN";

        /// <summary>
        /// Thông tin mở rộng của khách hàng dưới định dạng json
        /// </summary>
        public SocialProfile SocialProfile { get; set; } = new SocialProfile();

        /// <summary>
        /// Thời gian tạo entity
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Thời gian cập nhập cuối
        /// </summary>
        public DateTime UpdatedTime { get; set; }

    }
}