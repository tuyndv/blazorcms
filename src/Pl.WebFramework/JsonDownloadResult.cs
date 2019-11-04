using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pl.WebFramework
{
    public class JsonDownloadResult : IActionResult
    {
        public JsonDownloadResult(string json, string fileDownloadName)
        {
            Json = json;
            FileDownloadName = fileDownloadName;
        }

        public string FileDownloadName
        {
            get;
            set;
        }

        public string Json
        {
            get;
            set;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "text/json; charset=UTF-8";
            context.HttpContext.Response.Headers.Add("content-disposition", $"attachment; filename={FileDownloadName}");
            using MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(Json));
            return mStream.CopyToAsync(context.HttpContext.Response.Body);
        }
    }
}