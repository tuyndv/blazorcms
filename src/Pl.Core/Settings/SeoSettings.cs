namespace Pl.Core.Settings
{
    public class SeoSettings
    {
        /// <summary>
        /// Dấu phân cách của tiêu đề
        /// </summary>
        public string PageTitleSeparator { get; set; }

        /// <summary>
        /// Lên website luôn ở trước tiêu đề trang
        /// </summary>
        public bool SiteNameIsFist { get; set; }

        /// <summary>
        /// Cho phép url dạng unicode
        /// </summary>
        public bool AllowUnicodeCharsInUrls { get; set; }

        /// <summary>
        /// Hiển thị các mạng xã hội
        /// </summary>
        public bool EnableSocialNetworking { get; set; }

        /// <summary>
        /// Facebook
        /// </summary>
        public string FacebookLink { get; set; }

        /// <summary>
        /// Twitter
        /// </summary>
        public string TwitterLink { get; set; }

        /// <summary>
        /// Google
        /// </summary>
        public string GoogleLink { get; set; }

        /// <summary>
        /// Youtube
        /// </summary>
        public string YoutubeLink { get; set; }

        /// <summary>
        /// Rss
        /// </summary>
        public string Rss { get; set; }

        /// <summary>
        /// Hiển thị nút like facebook
        /// </summary>
        public bool FacebookLikeButton { get; set; }

        /// <summary>
        /// Hiển thị nút google cộng một
        /// </summary>
        public bool GoogleAddOneButton { get; set; }

        /// <summary>
        /// Hiển thị nút twitter
        /// </summary>
        public bool TwitterButton { get; set; }

        /// <summary>
        /// Hiển thị nút chia sẻ lên facebook
        /// </summary>
        public bool FacebookShareButton { get; set; }

        /// <summary>
        /// Hiển thị nút chia sẻ lên google
        /// </summary>
        public bool GoogleShareButton { get; set; }

        /// <summary>
        /// Hiển thị nút chia sẻ lên twitter
        /// </summary>
        public bool TwitterShareButton { get; set; }

        /// <summary>
        /// Hiển thị nút chia sẻ tổng hợp lên các mạng xã hội
        /// </summary>
        public bool AllShareButton { get; set; }

        /// <summary>
        /// Cho phép hiển thị mây thẻ
        /// </summary>
        public bool EnableTagCloud { get; set; }

        /// <summary>
        /// Tìm kiếm bằng google
        /// </summary>
        public bool SearchByGoogle { get; set; }

        /// <summary>
        /// Tên ảnh logo
        /// </summary>
        public string SiteLogoName { get; set; }

        /// <summary>
        /// Tên icon
        /// </summary>
        public string SiteIconName { get; set; }

        /// <summary>
        /// Site image
        /// </summary>
        public string SiteImageName { get; set; }

        /// <summary>
        /// Url địa chỉ gmap
        /// </summary>
        public string GmapUrl { get; set; }

        /// <summary>
        /// Mã html chèn thêm ờ footer
        /// </summary>
        public string FooterHtmlAdditional { get; set; }

        /// <summary>
        /// Phần thẻ html công thêm ở tiêu đề
        /// </summary>
        public string HeaderHtmlAdditional { get; set; }

        /// <summary>
        /// Nội dung file json google service sinh ra ở
        /// https://console.developers.google.com/iam-admin/serviceaccounts
        /// </summary>
        public string GoogleServiceAccountJson { get; set; }

        /// <summary>
        /// Cấu hình view id của google nanalytic muốn xem trong trang chủ admin
        /// </summary>
        public string GoogleAnalyticViewId { get; set; }
    }
}