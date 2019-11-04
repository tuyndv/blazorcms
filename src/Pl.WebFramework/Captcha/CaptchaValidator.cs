using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Pl.Core;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Pl.WebFramework.Captcha
{
    public class CaptchaValidator : Attribute, IActionFilter
    {
        public readonly string validUrl = "https://www.google.com/recaptcha/api/siteverify";
        private readonly string parameterName;

        /// <summary>
        /// Hàm validate captcha
        /// Được đặt ở đầu mỗi action khi cần validate
        /// True là valid và false là unvalid
        /// </summary>
        /// <param name="returnParameterName">Tên parameter nhận giá chị validate</param>
        public CaptchaValidator(string returnParameterName)
        {
            GuardClausesParameter.NullOrEmpty(returnParameterName, nameof(returnParameterName));
            parameterName = returnParameterName;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var systemSettings = context.HttpContext.RequestServices.GetService(typeof(ISystemSettings)) as ISystemSettings;

            if (!systemSettings.Captchar.Enabled)
            {
                context.ActionArguments[parameterName] = true;
                return;
            }

            bool valid = false;
            StringValues captcharResponse = context.HttpContext.Request.Form["g-recaptcha-response"];
            if (context.HttpContext.Request.Form.TryGetValue("g-recaptcha-response", out _))
            {
                using HttpClient client = new HttpClient();
                try
                {
                    Dictionary<string, string> pairs = new Dictionary<string, string>
                        {
                            { "secret", systemSettings.Captchar.SecretKey },
                            { "response", captcharResponse.ToString() },
                            { "remoteip", context.HttpContext.GetCurrentIpAddress() }
                        };
                    FormUrlEncodedContent formContent = new FormUrlEncodedContent(pairs);
                    HttpResponseMessage responseMessage = client.PostAsync(validUrl, formContent).Result;

                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        valid = (responseString.IndexOf("\"success\": true", StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                }
                catch
                {
                    valid = false;
                }
            }
            context.ActionArguments[parameterName] = valid;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}