using Microsoft.AspNetCore.Razor.TagHelpers;
using Pl.Core.Interfaces;

namespace Pl.WebFramework.Captcha
{
    /// <summary>
    /// Lớp tạo captchar cho hệ thống sử dụng recaptchar
    /// </summary>
    [HtmlTargetElement("recaptchar")]
    public class ReCaptchaTagHelper : TagHelper
    {
        private readonly ISystemSettings _systemSettings;

        public ReCaptchaTagHelper(ISystemSettings systemSettings)
        {
            _systemSettings = systemSettings;
        }

        [HtmlAttributeName("size")]
        public string Size { get; set; }

        [HtmlAttributeName("theme")]
        public string Theme { get; set; }

        [HtmlAttributeName("type")]
        public string Type { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            string dataSize = Size ?? _systemSettings.Captchar.Size;
            string dataType = Type ?? _systemSettings.Captchar.Type;
            string dataTheme = Theme ?? _systemSettings.Captchar.Theme;
            string htmlString = $"<script src=\"https://www.google.com/recaptcha/api.js\" async defer></script> <div class=\"g-recaptcha\" " +
                $"data-size=\"{dataSize}\" " +
                $"data-type=\"{dataType}\" " +
                $"data-theme=\"{dataTheme}\" " +
                $"data-sitekey=\"{_systemSettings.Captchar.SiteKey}\"></div>";
            output.Content.SetHtmlContent(htmlString);
            base.Process(context, output);
        }
    }
}