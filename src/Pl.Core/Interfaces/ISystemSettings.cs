using Pl.Core.Settings;

namespace Pl.Core.Interfaces
{

    public interface ISystemSettings
    {
        /// <summary>
        /// Thiết lập Common
        /// </summary>
        CommonSettings Common { get; set; }

        /// <summary>
        /// Thiết lập User
        /// </summary>
        UserSettings User { get; set; }

        /// <summary>
        /// Thiết lập Captchar
        /// </summary>
        CaptchaSettings Captchar { get; set; }

        /// <summary>
        /// Thiết lập Seo
        /// </summary>
        SeoSettings Seo { get; set; }

        /// <summary>
        /// Thiết lập Editor
        /// </summary>
        EditorSettings Editor { get; set; }

        /// <summary>
        /// Thiết lập Url
        /// </summary>
        UrlSettings Url { get; set; }

        /// <summary>
        /// Thiết lập Upload
        /// </summary>
        UploadSettings Upload { get; set; }
    }
}