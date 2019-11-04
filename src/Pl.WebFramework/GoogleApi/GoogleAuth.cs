using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace Pl.WebFramework.GoogleApi
{
    public class GoogleAuth
    {
        /// <summary>
        /// Lấy google accessToken dựa vào nội dung file json được export từ
        /// https://console.developers.google.com/projectselector/iam-admin/serviceaccounts
        /// </summary>
        /// <param name="jsonContentFile">Nội dung file json</param>
        /// <param name="scopes">Các quyền truy cập
        /// Vd analytics: https://www.googleapis.com/auth/analytics.readonly
        /// </param>
        /// <returns>AccessToken</returns>
        public async Task<string> GetAccessTokenFromJSONKeyAsync(string jsonContentFile, params string[] scopes)
        {
            return await GoogleCredential.FromJson(jsonContentFile).CreateScoped(scopes).UnderlyingCredential.GetAccessTokenForRequestAsync();
        }
    }
}