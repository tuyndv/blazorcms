using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Pl.Core.Entities;
using Pl.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pl.Core
{
    public static class CoreUtility
    {

        #region Object Helper

        /// <summary>
        /// Set một giá trị vào một thuộc tính của object
        /// </summary>
        /// <param name="instance">Object cần sét giá trị</param>
        /// <param name="propertyName">Tên thuộc tính</param>
        /// <param name="value">Giá trị cần sét</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            GuardClausesParameter.Null(instance, nameof(instance));
            GuardClausesParameter.NullOrEmpty(propertyName, nameof(propertyName));

            Type instanceType = instance.GetType();
            PropertyInfo pi = instanceType.GetProperty(propertyName);
            if (pi == null)
            {
                throw new Exception($"No property '{propertyName}' found on the instance of type '{instance.GetType()}'.");
            }

            if (!pi.CanWrite)
            {
                throw new Exception($"The property '{propertyName}' on the instance of type '{instance.GetType()}' does not have a setter.");
            }

            if (pi.PropertyType.IsConstructedGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && value != null)
            {
                value = Convert.ChangeType(value, Nullable.GetUnderlyingType(pi.PropertyType), CultureInfo.InvariantCulture);
            }

            pi.SetValue(instance, value, Array.Empty<object>());
        }

        /// <summary>
        /// Lấy giá trị của thuộc tính thuộc object
        /// </summary>
        /// <param name="instance">Object</param>
        /// <param name="propertyName">Tên thuộc tính</param>
        public static object GetProperty(object instance, string propertyName)
        {
            GuardClausesParameter.Null(instance, nameof(instance));
            GuardClausesParameter.NullOrEmpty(propertyName, nameof(propertyName));

            Type instanceType = instance.GetType();
            PropertyInfo pi = instanceType.GetProperty(propertyName);
            if (pi == null)
            {
                throw new Exception($"No property '{propertyName}' found on the instance of type '{instance.GetType()}'.");
            }

            if (!pi.CanWrite)
            {
                throw new Exception($"The property '{propertyName}' on the instance of type '{instance.GetType()}' does not have a setter.");
            }

            return pi.GetValue(instance, null);
        }

        /// <summary>
        /// Lấy giá trị của một thuộc tính trong object
        /// </summary>
        /// <typeparam name="T">Kiểu</typeparam>
        /// <param name="instance">Object chưa giá trị</param>
        /// <param name="propertyName">Tên thuộc tính cần lấy</param>
        public static T GetProperty<T>(object instance, string propertyName)
        {
            object value = GetProperty(instance, propertyName);
            return value == null ? default : (T)value;
        }

        /// <summary>
        /// Lấy danh sách các thuộc tính publish của object
        /// </summary>
        /// <param name="instance">Object</param>
        /// <returns>Danh sách thuộc tính</returns>
        public static List<PropertyInfo> GetPropertyList(object instance)
        {
            GuardClausesParameter.Null(instance, nameof(instance));

            Type instanceType = instance.GetType();
            return instanceType.GetProperties().ToList();
        }

        /// <summary>
        /// Hàn này binding dữ liệu từ objec một vào object 2
        /// </summary>
        /// <param name="readObject">Object đọc dữ liệu</param>
        /// <param name="saveObject">Object save dữ liệu</param>
        public static object BindingObjectData(object readObject, object saveObject)
        {
            if (readObject == null || saveObject == null)
            {
                return saveObject;
            }

            List<PropertyInfo> readPropertyList = GetPropertyList(readObject);
            List<PropertyInfo> savePropertyList = GetPropertyList(saveObject);
            IEnumerable<PropertyInfo> bindingPropertyList = from read in readPropertyList
                                                            join save in savePropertyList on read.Name equals save.Name
                                                            where read.GetType().Name == save.GetType().Name
                                                            where read.PropertyType.Equals(save.PropertyType)
                                                            where save.CanWrite
                                                            where read.CanRead
                                                            select read;
            foreach (PropertyInfo property in bindingPropertyList)
            {
                object value = GetProperty(readObject, property.Name);
                SetProperty(saveObject, property.Name, value);
            }
            return saveObject;
        }

        /// <summary>
        /// Hàn này binding dữ liệu từ objec một vào object 2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="readObject">Object đọc dữ liệu</param>
        /// <param name="saveObject">Object save dữ liệu</param>
        public static T BindingObjectData<T>(object readObject, object saveObject = null) where T : class
        {
            if (readObject == null)
            {
                return null;
            }

            if (saveObject == null)
            {
                saveObject = Activator.CreateInstance<T>();
            }

            return (T)BindingObjectData(readObject, saveObject);
        }

        /// <summary>
        /// Lấy thuộc tính ấn của một đối tượng
        /// </summary>
        /// <param name="target">Đối tượng</param>
        /// <param name="fieldName">Tên thộc tính</param>
        /// <returns>Object value</returns>
        public static object GetPrivateFieldValue(object target, string fieldName)
        {
            GuardClausesParameter.Null(target, nameof(target));
            GuardClausesParameter.NullOrEmpty(fieldName, nameof(fieldName));

            Type t = target.GetType();
            FieldInfo fi = null;

            while (t != null)
            {
                fi = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

                if (fi != null)
                {
                    break;
                }

                t = t.BaseType;
            }

            if (fi == null)
            {
                throw new Exception($"Field '{fieldName}' not found in type hierarchy.");
            }

            return fi.GetValue(target);
        }

        #endregion Object Helper

        #region Web Helper

        /// <summary>
        /// Kiểm tra xem yêu cầu của client gửi lên có phải là mobile hay không
        /// </summary>
        /// <param name="httpContext">Nội dung yêu cầu từ client</param>
        /// <returns>bool</returns>
        public static bool IsMobileRequest(this HttpContext httpContext)
        {
            string userAgent = httpContext?.Request?.Headers["User-Agent"];
            if (!string.IsNullOrEmpty(userAgent))
            {
                Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                return b.IsMatch(userAgent) || v.IsMatch(userAgent.Substring(0, 4));
            }
            return false;
        }

        /// <summary>
        /// Hàm cho biết request này có phải ajax hay không
        /// </summary>
        /// <param name="httpContext">http context</param>
        /// <returns>bool</returns>
        public static bool IsAjaxRequest(this HttpContext httpContext)
        {
            return (httpContext?.Request.Headers["X-Requested-With"] ?? "null") == "XMLHttpRequest";
        }

        /// <summary>
        /// Lấy đường dẫn liên quan của một Request
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>URL liên quan</returns>
        public static string GetUrlReferrer(this HttpContext httpContext)
        {
            return httpContext?.Request?.Headers["Referer"].ToString() ?? string.Empty;
        }

        /// <summary>
        /// Lấy IP của máy client gửi yêu cầu đến máy chủ
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>Id của máy khách</returns>
        public static string GetCurrentIpAddress(this HttpContext httpContext)
        {
            return httpContext?.Connection?.RemoteIpAddress.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Lấy đường dẫn hiện tại của request
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="includeQueryString">Có kèm theo các query string không</param>
        /// <returns>Đường dẫn web</returns>
        public static string GetThisPageUrl(this HttpContext httpContext, bool includeQueryString = true)
        {
            if (null == httpContext)
            {
                return string.Empty;
            }

            if (includeQueryString)
            {
                return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
            }
            else
            {
                return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}";
            }
        }

        /// <summary>
        /// Lấy trạng thái bảo mật của yêu cầu
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>bool</returns>
        public static bool IsCurrentConnectionSecured(this HttpContext httpContext)
        {
            return httpContext?.Request.IsHttps ?? false;
        }

        /// <summary>
        /// Trả về true nếu url yêu cầu là đến một tài nguyên tĩnh không cần phải webserver xử lý
        /// Nguồn https://github.com/aspnet/StaticFiles/blob/dev/src/Microsoft.AspNetCore.StaticFiles/FileExtensionContentTypeProvider.cs
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>bool</returns>
        public static bool IsStaticResource(this HttpContext httpContext)
        {
            string path = httpContext?.Request.Path;
            FileExtensionContentTypeProvider contentTypeProvider = new FileExtensionContentTypeProvider();
            return contentTypeProvider.TryGetContentType(path, out _);
        }

        /// <summary>
        /// Lấy domain của website
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>domain hệ thống</returns>
        public static string GetWebDomain(this HttpContext httpContext)
        {
            return $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}";
        }

        /// <summary>
        /// Lấy giá trị của QueryString
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="queryString">Tên của query string cần lấy</param>
        /// <returns>Giá trị string</returns>
        public static string GetQueryString(this HttpContext httpContext, string queryString)
        {
            var url = httpContext.GetThisPageUrl();

            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }

            Uri uri = new Uri(url);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue(queryString, out StringValues value);
            return value;
        }

        /// <summary>
        /// Lấy giá trị của QueryString
        /// </summary>
        /// <param name="uri">Url cần lấy query string</param>
        /// <param name="queryString">Tên của query string cần lấy</param>
        /// <returns>Giá trị string</returns>
        public static string GetQueryString(Uri uri, string queryString)
        {
            QueryHelpers.ParseQuery(uri?.Query).TryGetValue(queryString, out StringValues value);
            return value;
        }

        /// <summary>
        /// Làm mới một giá trị query string trên url
        /// </summary>
        /// <param name="url">Một url cần làm mới</param>
        /// <param name="queryStringModification">Tên query string</param>
        /// <param name="value">Giá trị làm mới</param>
        /// <returns>Đường dẫn url mới</returns>
        public static string ModifyQueryString(string url, string queryStringModification, string value)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            var parametersToAdd = new Dictionary<string, string> { { queryStringModification, value } };
            return QueryHelpers.AddQueryString(url, parametersToAdd);
        }

        /// <summary>
        /// Gỡ bỏ một query string trên url
        /// </summary>
        /// <param name="uri">Đường dẫn url</param>
        /// <param name="queryString">Tên query string cần loại bỏ</param>
        /// <returns>Đường dẫn url mới</returns>
        public static string RemoveQueryString(Uri uri, string queryString)
        {
            QueryHelpers.ParseQuery(uri?.Query).Remove(queryString);
            return uri?.ToString();
        }

        /// <summary>
        /// Lấy giá trị cookie bằng key
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="key">Key</param>
        /// <returns>Trả về dạng string</returns>
        public static string GetCookieByKey(this HttpContext httpContext, string key)
        {
            string cookie = httpContext?.Request.Cookies[key];
            if (cookie != null)
            {
                return cookie;
            }
            return string.Empty;
        }

        /// <summary>
        /// Sét giá trị vào cookies
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="key">Key</param>
        /// <param name="value">Giá trị</param>
        /// <param name="expiresTime">Thời gian hết hạn tính bằng giây</param>
        /// <param name="path">Đường dẫn</param>
        public static void SetCookieByKey(this HttpContext httpContext, string key, string value, long? expiresTime = null, string path = "")
        {
            if (httpContext != null)
            {
                CookieOptions option = new CookieOptions()
                {
                    HttpOnly = false
                };
                if (expiresTime.HasValue)
                {
                    option.Expires = DateTime.Now.AddSeconds(expiresTime.Value);
                }

                if (string.IsNullOrEmpty(path))
                {
                    option.Path = path;
                }

                httpContext.Response.Cookies.Delete(key);
                httpContext.Response.Cookies.Append(key, value, option);
            }
        }

        /// <summary>
        /// Làm đường dẫn upload file và tạo thư mục sẵn sàng upload
        /// Hàm này có sửa phải đồng bộ với workService.cs
        /// </summary>
        /// <param name="rootPath">Thư mục upload gốc</param>
        /// <param name="uploadDate"></param>
        /// <param name="size">Các kích thước thumb nếu có</param>
        /// <returns>string</returns>
        public static string BuildUploadPath(string rootPath, DateTime? uploadDate = null, string size = "")
        {
            if (uploadDate == null)
            {
                uploadDate = DateTime.Now;
            }

            var fullPath = rootPath + size + uploadDate.Value.ToString("/yyyy/MM/dd/", CultureInfo.InvariantCulture).Replace('/', '\\');
            fullPath = Path.Combine(fullPath);
            CreateDirectory(fullPath);
            return fullPath;
        }

        /// <summary>
        /// Chuyển một html content ra string
        /// </summary>
        /// <param name="htmlContent">Khối html</param>
        /// <returns>string</returns>
        public static string ToHtmlString(this IHtmlContent htmlContent)
        {
            using StringWriter writer = new StringWriter();
            htmlContent?.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        #endregion Web Helper

        #region Number Helper

        /// <summary>
        /// Tạo một số int ngẫu nhiên
        /// </summary>
        /// <param name="min">Giới hạn nhỏ nhất</param>
        /// <param name="max">Giới hạn lớn nhất</param>
        /// <returns>int</returns>
        public static int RandomInteger(int min = 0, int max = int.MaxValue)
        {
            Random rng = new Random(DateTime.Now.Millisecond);
            return rng.Next(min, max);
        }

        /// <summary>
        /// Sinh một chuỗi số ngẫu nhiên
        /// </summary>
        /// <param name="length">Độ dài</param>
        /// <returns>Result string</returns>
        public static string RandomDigitCode(int length)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            string str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                str = string.Concat(str, random.Next(9));
            }

            return str;
        }

        /// <summary>
        /// Lấy số nhỏ nhất trong 3 số
        /// </summary>
        /// <param name="a">Số đầu tiên</param>
        /// <param name="b">Số thứ 2</param>
        /// <param name="c">Số thứ 3</param>
        /// <returns>int</returns>
        public static int Min3(int a, int b, int c)
        {
            int min;
            if (a > b)
            {
                min = b;
            }
            else
            {
                min = a;
            }

            if (min > c)
            {
                min = c;
            }

            return min;
        }

        #endregion Number Helper

        #region String Helper

        /// <summary>
        /// Chuyển một đoạn text có các ký tự đặc biệt về base 64bit
        /// </summary>
        /// <param name="plainText">Text thường</param>
        /// <returns>string base64</returns>
        public static string Base64Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Chuyển một đoạn tring từ base 64 thành chuỗi thường
        /// </summary>
        /// <param name="base64EncodedData">Chuỗi base 64</param>
        /// <returns>string chuỗi thường</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return string.Empty;
            }

            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Hàm cắt ngắn một chuỗi
        /// Nếu nẻ một chữ thì bỏ chữ đó cho đến dấu khoảng cách cuối cùng
        /// </summary>
        /// <param name="sentence">Chuỗi cần cắt</param>
        /// <param name="len">Độ dài</param>
        /// <param name="expanded"></param>
        /// <returns>Chuỗi cộng thêm sau khi cắt ngắn</returns>
        public static string TruncateString(string sentence, int len, string expanded = "...")
        {
            if (sentence == null)
            {
                return string.Empty;
            }

            len -= expanded?.Length ?? 0;

            if (sentence.Length > len)
            {
                sentence = sentence.Substring(0, len);
                int pos = sentence.LastIndexOf(' ');
                if (pos > 0)
                {
                    sentence = sentence.Substring(0, pos);
                }

                return sentence + expanded;
            }
            return sentence;
        }

        /// <summary>
        /// Chuyển ký tự enter thàng thẻ br trong html
        /// </summary>
        /// <param name="strContent">Chuỗi nội dung</param>
        /// <returns>string</returns>
        public static string ReplaceLineBreak(string strContent)
        {
            return !string.IsNullOrEmpty(strContent) ? Regex.Replace(strContent, @"\t|\n|\r", "<br />") : strContent;
        }

        /// <summary>
        /// Chuyển các thẻ html thành các ký tự đánh dấu
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string StripTags(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = Regex.Replace(text, @"(>)(\r|\n)*(<)", "><");
            text = Regex.Replace(text, "(<[^>]*>)([^<]*)", "$2");
            return Regex.Replace(text, "(&#x?[0-9]{2,4};|&quot;|&amp;|&nbsp;|&lt;|&gt;|&euro;|&copy;|&reg;|&permil;|&Dagger;|&dagger;|&lsaquo;|&rsaquo;|&bdquo;|&rdquo;|&ldquo;|&sbquo;|&rsquo;|&lsquo;|&mdash;|&ndash;|&rlm;|&lrm;|&zwj;|&zwnj;|&thinsp;|&emsp;|&ensp;|&tilde;|&circ;|&Yuml;|&scaron;|&Scaron;)", "@");
        }

        /// <summary>
        /// Loại bỏ thẻ a trong một chuỗi html
        /// Ví dụ đầu vào <a href="http://example.com">Name</a> đầu ra "Name")
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns>Text string</returns>
        public static string ReplaceAnchorTags(string htmlString)
        {
            return string.IsNullOrEmpty(htmlString)
                ? string.Empty
                : Regex.Replace(htmlString, @"<a\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Hàm loại bỏ các ký tự unicode sang các kí tự thường.
        /// </summary>
        /// <param name="value">Chuỗi unicode</param>
        /// <returns>string</returns>
        public static string RemoveUnicode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            value = Regex.Replace(value, @"\s+", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            string nfd = value.Normalize(NormalizationForm.FormD);
            StringBuilder retval = new StringBuilder(nfd.Length);
            foreach (char ch in nfd)
            {
                if (ch >= '\u0300' && ch <= '\u036f')
                {
                    continue;
                }

                if (ch >= '\u1dc0' && ch <= '\u1de6')
                {
                    continue;
                }

                if (ch >= '\ufe20' && ch <= '\ufe26')
                {
                    continue;
                }

                if (ch >= '\u20d0' && ch <= '\u20f0')
                {
                    continue;
                }

                retval.Append(ch);
            }
            return retval.ToString();
        }

        /// <summary>
        /// Tạo url bằng một đoạn unicode string
        /// </summary>
        /// <param name="text">Chuỗi string</param>
        /// <param name="maxLength">Độ dài url tối đa</param>
        /// <returns>string</returns>
        public static string UrlFromUnicode(string text, int maxLength = 150)
        {
            if (text == null) return "";
            var normalizedString = text.ToUpperInvariant().Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            var stringLength = normalizedString.Length;
            var prevdash = false;
            var trueLength = 0;
            char c;
            for (int i = 0; i < stringLength; i++)
            {
                c = normalizedString[i];
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (c < 128)
                            stringBuilder.Append(c);
                        else
                            stringBuilder.Append(RemapInternationalCharToAscii(c));
                        prevdash = false;
                        trueLength = stringBuilder.Length;
                        break;
                    case UnicodeCategory.SpaceSeparator:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.DashPunctuation:
                    case UnicodeCategory.OtherPunctuation:
                    case UnicodeCategory.MathSymbol:
                        if (!prevdash)
                        {
                            stringBuilder.Append('-');
                            prevdash = true;
                            trueLength = stringBuilder.Length;
                        }
                        break;
                }
                if (maxLength > 0 && trueLength >= maxLength)
                    break;
            }
            var result = stringBuilder.ToString().Trim('-').ToLower(CultureInfo.InvariantCulture);
            return maxLength <= 0 || result.Length <= maxLength ? result : result.Substring(0, maxLength);
        }

        /// <summary>
        /// Chuyển ký tự unicode sang ascii
        /// </summary>
        /// <param name="c">Ký tự</param>
        /// <returns>string</returns>
        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString(CultureInfo.InvariantCulture).ToUpperInvariant();
            if ("àåáâäãåą".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "c";
            }
            else if ("żźž".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "s";
            }
            else if ("ñń".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s, StringComparison.OrdinalIgnoreCase))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Bỏ các ký tự đặc biệt nhưng k bỏ unicode
        /// </summary>
        /// <param name="value">Chuỗi string</param>
        /// <param name="len">độ dài</param>
        /// <returns>string</returns>
        public static string RemoveSpecialNotUnicode(string value, int len = 150)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (len > 0)
            {
                value = TruncateString(value, len, "");
            }

            for (int i = 32; i < 48; i++)
            {
                value = value.Replace(((char)i).ToString(CultureInfo.InvariantCulture), "-", StringComparison.OrdinalIgnoreCase);
            }

            value = Regex.Replace(value, @"\s+", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            value = value.Replace(".", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace(" ", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace(",", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace(";", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace(":", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace("?", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace("!", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace("“", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace("”", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace("]", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace("[", "-", StringComparison.OrdinalIgnoreCase);
            value = value.Replace("\t", "-", StringComparison.OrdinalIgnoreCase);
            while (value.Contains("--", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Replace("--", "-", StringComparison.OrdinalIgnoreCase);
            }

            return value.Trim('-');
        }

        /// <summary>
        /// Hàm tạo tên file từ một tên unicode
        /// </summary>
        /// <param name="fileNameFull"></param>
        /// <param name="len">độ dài</param>
        /// <returns>string</returns>
        public static string FileNameFromUnicode(string fileNameFull, int len = 150)
        {
            if (string.IsNullOrEmpty(fileNameFull))
            {
                return string.Empty;
            }

            string fileNameOnly = Path.GetFileNameWithoutExtension(fileNameFull);
            string extension = Path.GetExtension(fileNameFull);
            string path = Path.GetDirectoryName(fileNameFull);

            if (len > 0)
            {
                fileNameOnly = TruncateString(fileNameOnly, len, "");
            }

            fileNameOnly = UrlFromUnicode(fileNameOnly);
            string newFileName = Regex.Replace(fileNameOnly, "[-]{2,}", "-");
            return Path.Combine(path, newFileName + extension);
        }

        /// <summary>
        /// Sinh chuỗi tự động với độ dài được cố định
        /// </summary>
        /// <param name="size">Độ dài</param>
        /// <returns>string</returns>
        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Loại bỏ toàn bộ các ký tự không phải là ký tự sổ ra khỏi một chuỗi
        /// </summary>
        /// <param name="data">Dữ liệu ban đầu</param>
        /// <returns>string</returns>
        public static string RemoveAllNonAlphanumericCharacters(string data)
        {
            Regex rgx = new Regex("[^0-9.]");
            return rgx.Replace(data, "");
        }

        /// <summary>
        /// Loại bỏ toàn bộ các ký tự sổ ra khỏi một chuỗi
        /// </summary>
        /// <param name="data">Dữ liệu ban đầu</param>
        /// <returns>string</returns>
        public static string RemoveAllAlphanumericCharacters(string data)
        {
            Regex rgx = new Regex("[0-9]");
            return rgx.Replace(data, "");
        }

        /// <summary>
        /// Hàm hỗ trợ replace nội dung
        /// </summary>
        /// <param name="content">Nôi dung</param>
        /// <param name="keyAndValue">Cặp giá trị và key</param>
        /// <returns>Nội dung</returns>
        public static string ReplaceContentHelper(string content, Dictionary<string, string> keyAndValue)
        {
            if (keyAndValue != null)
            {
                foreach (KeyValuePair<string, string> item in keyAndValue)
                {
                    content = content?.Replace(item.Key, item.Value, StringComparison.OrdinalIgnoreCase);
                }
            }
            return content;
        }

        /// <summary>
        /// Lấy danh sách thẻ trong html string
        /// </summary>
        /// <param name="str">html string</param>
        /// <param name="tagName">Tên thẻ cần lấy</param>
        /// <returns>Danh sách thẻ lấy được từ html string</returns>
        public static List<string> GetListTag(string str, string tagName = "")
        {
            if (str == null || str.Trim()?.Length == 0)
            {
                return null;
            }

            Match mt = (new Regex("'<[^<]*?>'")).Match(str);
            while (mt.Success)
            {
                str = str.Replace(mt.Value, "", StringComparison.OrdinalIgnoreCase);
                mt = mt.NextMatch();
            }

            mt = (new Regex("(<(?<tag>[^<]*?)\">|<(?<tag>[^<]*?)>)")).Match(str);
            List<string> listTag = new List<string>();
            while (mt.Success)
            {
                if (string.IsNullOrEmpty(tagName))
                {
                    listTag.Add(mt.Value);
                }
                else
                    if (mt.Value.ToUpperInvariant().StartsWith("<" + tagName, StringComparison.OrdinalIgnoreCase))
                {
                    listTag.Add(mt.Value);
                }

                mt = mt.NextMatch();
            }

            return listTag;
        }

        /// <summary>
        /// Lấy danh sách url ảnh trong một string content html
        /// </summary>
        /// <param name="str">string content html</param>
        /// <returns>List string</returns>
        public static List<string> GetListImageUrl(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<string>();
            }

            List<string> imgList = new List<string>();
            List<string> listTag = GetListTag(str, "img");
            if (listTag?.Count > 0)
            {
                for (int i = 0; i < listTag.Count; i++)
                {
                    string item = listTag[i];
                    Match mt = (new Regex("src=\"(?<src>.*?)\"", RegexOptions.IgnoreCase)).Match(item);
                    string src = mt.Success ? mt.Groups["src"].Value : string.Empty;

                    if (!string.IsNullOrEmpty(src))
                    {
                        imgList.Add(src);
                    }
                }
            }

            return imgList;
        }

        /// <summary>
        /// Lấy danh sách url trong một string content html
        /// </summary>
        /// <param name="str">string content html</param>
        /// <returns>List string</returns>
        public static List<string> GetListLinkUrl(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new List<string>();
            }

            List<string> urlList = new List<string>();
            List<string> listTag = GetListTag(str, "a");
            if (listTag?.Count > 0)
            {
                for (int i = 0; i < listTag.Count; i++)
                {
                    string item = listTag[i];
                    Match mt = (new Regex("href=\"(?<href>.*?)\"", RegexOptions.IgnoreCase)).Match(item);
                    string href = mt.Success ? mt.Groups["href"].Value : string.Empty;

                    if (!string.IsNullOrEmpty(href))
                    {
                        urlList.Add(href);
                    }
                }
            }

            return urlList;
        }

        /// <summary>
        /// Chuyển một đoạn text có nhiều dòng, lộn xộn các khoảng cách thành đoạn text chỉ có một dòng duy nhất và rất là chật tự
        /// </summary>
        /// <param name="content">Nội dung đoạn text</param>
        /// <returns>string</returns>
        public static string CompressorString(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = Regex.Replace(content, @"\t|\n|\r", " ");//Loại bỏ các kí tự xuống dòng
                content = Regex.Replace(content, "[ ]{2,}", " ");//chuyển nhiều hơn 2 ký tự khoảng trắng về làm một
                content = content.Trim();
            }
            return content;
        }

        /// <summary>
        /// So sánh độ tương đồng giữa 2 chuỗi với nhau
        /// </summary>
        /// <param name="s">Chuỗi thứ nhất</param>
        /// <param name="t">Chuỗi thứ 2</param>
        /// <returns>int</returns>
        private static int ComputeDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] distance = new int[n + 1, m + 1];
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; distance[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; distance[0, j] = j++)
            {
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);
                    distance[i, j] = Min3(distance[i - 1, j] + 1, distance[i, j - 1] + 1, distance[i - 1, j - 1] + cost);
                }
            }
            return distance[n, m];
        }

        /// <summary>
        /// Tính điểm tương đồng giừa hai chuỗi ký tự
        /// </summary>
        /// <param name="source">Chuỗi gốc</param>
        /// <param name="dest">Chuỗi cần so sánh</param>
        /// <returns>double</returns>
        public static double SimilarityScore(string source, string dest)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(dest))
            {
                return 0;
            }

            int distance = ComputeDistance(source ?? "", dest ?? "");
            return 1 - (2.0 * distance / (source?.Length ?? 0 + dest?.Length ?? 0));
        }

        /// <summary>
        /// Cắt một bỏ một đoạn ở cuối một chuỗi
        /// </summary>
        /// <param name="input">Chuỗi ban đầu</param>
        /// <param name="suffixToRemove">Chuỗi cần cắt bỏ</param>
        /// <param name="comparisonType">Loại so sánh chuỗi cắt bỏ</param>
        /// <returns>string</returns>
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            else
            {
                return input;
            }
        }

        #endregion String Helper

        #region File Helper

        /// <summary>
        /// Hàm download một file bằng web request
        /// </summary>
        /// <param name="uri">Url trỏ đến file, hoặc stream của file</param>
        /// <param name="outputFilePath">Đường dẫn để lưu lại file</param>
        public static async Task<bool> DownloadFile(Uri uri, string outputFilePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(outputFilePath);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                using HttpClient client = new HttpClient();
                File.WriteAllBytes(outputFilePath, await client.GetByteArrayAsync(uri));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Hàm ghi file theo mảng bytes
        /// </summary>
        /// <param name="bytes">Mảng byte</param>
        /// <param name="outputFilePath">Đường dẫn đề ghi vào</param>
        /// <returns>bool</returns>
        public static bool SaveFileFromBytes(byte[] bytes, string outputFilePath)
        {
            try
            {
                if (bytes != null)
                {
                    FileInfo fileInfo = new FileInfo(outputFilePath);
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }

                    using (FileStream fileStream = new FileStream(outputFilePath, FileMode.Create))
                    {
                        fileStream.Write(bytes, 0, bytes.Length);
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa file theo đường dẫn. Hàm không thow lỗi nếu có
        /// </summary>
        /// <param name="filePart">Đường dẫn file</param>
        /// <returns>bool</returns>
        public static bool DeleteFile(string filePart)
        {
            FileInfo _file = new FileInfo(filePart);
            if (_file.Exists)
            {
                _file.Delete();
            }

            return true;
        }

        /// <summary>
        /// Thử cố ghi vào thực mục nào đó
        /// </summary>
        /// <param name="path">Thư mục cần gi</param>
        /// <returns>bool</returns>
        public static bool TryWriteDirectory(string path)
        {
            try
            {
                Directory.SetLastWriteTimeUtc(path, DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thử cố ghi vào thực mục nào đó
        /// </summary>
        /// <param name="path">File cần gi</param>
        /// <returns>bool</returns>
        public static bool TryWriteFile(string path)
        {
            try
            {
                File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thử kiểm tra xem thư mục đã tồn tại và có thể ghi hay không
        /// </summary>
        /// <param name="path">Thư mục cần gi</param>
        /// <returns>bool</returns>
        public static bool CheckDirectoryReady(string path)
        {
            try
            {
                CreateDirectory(path);
                string testFilePath = path + "\\checkwrite.temp";
                File.WriteAllText(testFilePath, "Test write file");
                File.Delete(testFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Hàm kiểm tra và đổi tên file nếu file đã tồn tại. Hàm sẽ đổi tên file mới để cho save không bị đè.
        /// </summary>
        /// <param name="fullPath">Full đường dẫn</param>
        /// <param name="newNameFormat">Định dạng tên mới với 0 là phần tên và 1 là phần tên được thêm mới</param>
        /// <returns>Tên mới</returns>
        public static string IdentityFileName(string fullPath, string newNameFormat = "{0}({1})")
        {
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                return string.Empty;
            }

            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format(CultureInfo.InvariantCulture, newNameFormat, fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }
            return newFullPath;
        }

        /// <summary>
        /// Check xem đã có thư mục chưa thì phải tạo thư mục.
        /// </summary>
        /// <param name="path"></param>
        public static bool CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Hàm lấy đuôi mở rộng file từ một đường dẫn thư mục hoặc url
        /// có chứa dấu .
        /// </summary>
        /// <param name="path">Đường dẫn thư mục hoặc url</param>
        /// <param name="withDot"></param>
        /// <returns>string</returns>
        public static string GetFileExtension(string path, bool withDot = true)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            try
            {
                string extension = Path.GetExtension(path);
                return withDot ? extension : extension.TrimStart('.');
            }
            catch (UriFormatException)
            {
                Uri uri = new Uri(path);
                string extension = Path.GetExtension(uri.LocalPath);
                return withDot ? extension : extension.TrimStart('.');
            }
        }

        /// <summary>
        /// Hàm lấy tên file từ một đường dẫn thư mục hoặc url
        /// </summary>
        /// <param name="path">Đường dẫn thư mục hoặc url</param>
        /// <returns>string</returns>
        public static string GetFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            try
            {
                return Path.GetFileName(path);
            }
            catch (UriFormatException)
            {
                Uri uri = new Uri(path);
                return Path.GetFileName(uri.LocalPath);
            }
        }

        /// <summary>
        /// Lấy kích thước một file dựa vào đường dẫn trực tiếp đến file
        /// </summary>
        /// <param name="path">Đường dẫn</param>
        /// <returns>1- là không có file hoặc lỗi</returns>
        public static long GetFileSize(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return -1;
            }

            FileInfo file = new FileInfo(path);
            return file.Exists ? file.Length : -1;
        }

        /// <summary>
        /// Lấy danh sách tên file trong một thư mục
        /// </summary>
        /// <param name="directoryPath">Thư mục cần lấy danh sách file</param>
        /// <param name="searchPattern">Lọc file</param>
        /// <param name="topDirectoryOnly">Chỉ lấy đúng thư mục hay lấy cả các thư muc con. Mặc định là lấy đúng 1 thư mục chỉ định</param>
        /// <returns>IEnumerable string</returns>
        public static IEnumerable<string> ListFileNames(string directoryPath, string searchPattern, bool topDirectoryOnly = true)
        {
            return Directory.EnumerateFiles(directoryPath, searchPattern, topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
        }

        #endregion File Helper

        #region Tree Helper

        /// <summary>
        /// Xây dựng cây danh sách
        /// </summary>
        /// <typeparam name="T">Loại object</typeparam>
        /// <typeparam name="K">Thuộc tính object</typeparam>
        /// <param name="collection">Danh sách cần tạo tree</param>
        /// <param name="idSelector">Thuộc tính id của object</param>
        /// <param name="parentIdSelector">Thuộc tính cha id</param>
        /// <param name="orderSelector">Thuộc tính xắp xếp thứ tự</param>
        /// <param name="rootId">Giá trị gốc</param>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K, O>(this IEnumerable<T> collection, Func<T, K> idSelector, Func<T, K> parentIdSelector, Func<T, O> orderSelector, K rootId = default)
        {
            foreach (T c in collection.Where(c => parentIdSelector(c).Equals(rootId)).OrderBy(q => orderSelector(q)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(idSelector, parentIdSelector, orderSelector, idSelector(c))
                };
            }
        }

        #endregion Tree Helper

        #region Email Helper

        /// <summary>
        /// Hàm gửi email
        /// </summary>
        /// <param name="userName">Tài khoản</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="subject">Tiêu đề</param>
        /// <param name="body">Nội dung</param>
        /// <param name="from">Gửi từ</param>
        /// <param name="from">Tên người gửi</param>
        /// <param name="fromName"></param>
        /// <param name="tos">Gửi đến</param>
        /// <param name="host">Server mail</param>
        /// <param name="port">Cổng</param>
        /// <param name="isSsl">Có sử dụng ssl hay không</param>
        /// <param name="isUseDefauletCredentials">Sử dụng tài khoản mặc định hay k</param>
        /// <param name="bcc">Danh sách email bcc</param>
        /// <param name="cc">Danh sách email cc</param>
        /// <param name="attachCollection">Đính kèm theo</param>
        public static async Task<bool> SendEmail(string userName, string password, string subject, string body, string from, string fromName, List<string> tos, string host, int port, bool isSsl, bool isUseDefauletCredentials = false, List<string> bcc = null, List<string> cc = null, AttachmentCollection attachCollection = null)
        {
            if (string.IsNullOrEmpty(body) || string.IsNullOrEmpty(from) || tos == null || tos.Count <= 0 || port <= 0 || string.IsNullOrEmpty(host))
            {
                return false;
            }

            if (!isUseDefauletCredentials)
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    return false;
                }
            }

            MailMessage message = new MailMessage
            {
                From = new MailAddress(from, fromName)
            };
            foreach (string address in tos.Where(to => !string.IsNullOrWhiteSpace(to)))
            {
                message.To.Add(address.Trim());
            }

            if (bcc != null)
            {
                foreach (string address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }

            if (cc != null)
            {
                foreach (string address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            if (attachCollection != null)
            {
                foreach (Attachment attach in attachCollection)
                {
                    message.Attachments.Add(attach);
                }
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Host = host;
                smtpClient.Port = port;
                smtpClient.EnableSsl = isSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                if (isUseDefauletCredentials)
                {
                    smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    smtpClient.Credentials = new NetworkCredential(userName, password);
                }

                await smtpClient.SendMailAsync(message);
                message.Dispose();
            }
            return true;
        }

        #endregion Email Helper

        #region DateTime Helper

        /// <summary>
        /// Hàm chuyển thời gian thành chữ cho rễ đọc
        /// </summary>
        /// <param name="dateTime">Thời gian</param>
        /// <param name="secondsText">Mấy giây trước</param>
        /// <param name="minutesText">Mấy phút trước</param>
        /// <param name="hoursText">Mấy giờ trước</param>
        /// <param name="yesterdayText">Ngày hôm qua</param>
        /// <param name="dateTimeFormat">Định dạng ngày thánh</param>
        /// <returns>string</returns>
        public static string GetTextTime(DateTime dateTime, string secondsText = " giây trước", string minutesText = " phút trước", string hoursText = " giờ trước", string yesterdayText = "hôm qua, ", string dateTimeFormat = "")
        {
            if (dateTime > DateTime.Now)
            {
                return dateTime.ToString(CultureInfo.CurrentCulture);
            }

            double timeDifference = (DateTime.Now - dateTime).TotalSeconds;
            if (timeDifference < 60)
            {
                return (long)timeDifference + secondsText;
            }

            if (timeDifference < 3600)
            {
                return (long)(DateTime.Now - dateTime).TotalMinutes + minutesText;
            }

            if (timeDifference < 86400)
            {
                return (long)(DateTime.Now - dateTime).TotalHours + hoursText;
            }

            if (timeDifference < 172800)
            {
                return yesterdayText + dateTime.ToString("h:mm:ss tt", CultureInfo.CurrentCulture);
            }

            return !string.IsNullOrWhiteSpace(dateTimeFormat) ? dateTime.ToString(dateTimeFormat, CultureInfo.CurrentCulture) : dateTime.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Hàm lấy danh sách các ngày trong tuần
        /// Hàm mặc định lấy vùng miên mặc định
        /// </summary>
        /// <param name="oneDayInWeek">Một ngày bất kỳ trong tuần đó, Nếu để trống là tuần hiện tại</param>
        /// <param name="startOfWeek"></param>
        /// <returns>List DateTime</returns>
        public static List<DateTime> GetWeekDay(DateTime? oneDayInWeek = null, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            DateTime tempDate = oneDayInWeek ?? DateTime.Now;
            DateTime copyDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day);
            int diff = copyDate.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            DateTime startDate = copyDate.AddDays(-diff).Date;

            //listWeekDay.Add(new DateTime(startDate.Year, startDate.Month, 7).AddMilliseconds(-1));
            return Enumerable.Range(0, 7).Select(q => startDate.AddDays(q)).ToList();
        }

        /// <summary>
        /// Hàm lấy ngày bắt đầu tuần và kết thúc tuần
        /// </summary>
        /// <param name="dayInWeek">Ngày trong tuần</param>
        /// <param name="start">Ngày bắt đầu</param>
        /// <param name="end">Ngày kết thúc</param>
        /// <param name="startOfWeek">Ngày bắt đầu tuần</param>
        public static void GetWeekStartAndEndDay(DateTime dayInWeek, out DateTime start, out DateTime end, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            List<DateTime> listDayOfWeek = GetWeekDay(dayInWeek, startOfWeek);
            start = listDayOfWeek.FirstOrDefault();
            end = listDayOfWeek.LastOrDefault();
        }

        /// <summary>
        /// Hàm lấy ngày bắt đầu ngày và kết thúc ngày
        /// </summary>
        /// <param name="timeInDay">Thời giam trong ngày</param>
        /// <param name="start">Thời gian bắt đầu</param>
        /// <param name="end">Thời gian kết thúc</param>
        public static void GetDayStartAndEndTime(DateTime timeInDay, out DateTime start, out DateTime end)
        {
            start = new DateTime(timeInDay.Year, timeInDay.Month, timeInDay.Day);
            end = start.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Hàm lấy ngày bắt đầu tháng và kết thúc tháng
        /// </summary>
        /// <param name="dayInMonth">Ngày trong tháng</param>
        /// <param name="start">Ngày bắt đầu</param>
        /// <param name="end">Ngày kết thúc</param>
        public static void GetMonthStartAndEndDay(DateTime dayInMonth, out DateTime start, out DateTime end)
        {
            DateTime copyDate = new DateTime(dayInMonth.Year, dayInMonth.Month, dayInMonth.Day);
            start = copyDate.AddDays(1 - copyDate.Day);
            end = start.AddMonths(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Hàm lấy ngày bắt đầu năm và kết thúc năm
        /// </summary>
        /// <param name="dayInYear">Ngày trong năm</param>
        /// <param name="start">Ngày bắt đầu</param>
        /// <param name="end">Ngày kết thúc</param>
        public static void GetYearStartAndEndDay(DateTime dayInYear, out DateTime start, out DateTime end)
        {
            start = new DateTime(dayInYear.Year, 1, 1);
            end = start.AddYears(1).AddMilliseconds(-1);
        }

        #endregion DateTime Helper
    }

    public class MatchExpression
    {
        /// <summary>
        /// Danh sách regex có cần kiểm tra
        /// </summary>
        public List<Regex> Regexes { get; } = new List<Regex>();

        /// <summary>
        /// hành động được thực hiện khi có regex math
        /// </summary>
        public Func<Match, string> Action { get; set; }
    }
}
