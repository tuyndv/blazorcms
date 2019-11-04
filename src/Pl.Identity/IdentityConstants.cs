using Pl.Core;

namespace Pl.Identity
{
    public static class IdentityConstants
    {
        /// <summary>
        /// Save login info default time. Default value is a year
        /// </summary>
        public const long RememberLoginTime = 2592000;

        /// <summary>
        /// Current language key
        /// </summary>
        public static readonly string LanguageSessionCookieKey = $"{ObjectTypeEnum.User}_sclk";

        /// <summary>
        /// Current cookie key
        /// </summary>
        public static readonly string CurencySessionCookieKey = $"{ObjectTypeEnum.User}_scck";

    }
}
