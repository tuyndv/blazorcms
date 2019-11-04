using Microsoft.Extensions.Logging;
using Pl.Core.Entities;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Pl.Core.Services
{
    public class NotificationService : INotificationService
    {
        #region Properties And Constructor
        private readonly ILogger<NotificationService> _logger;
        private readonly ISystemSettings _systemSettings;

        public NotificationService(
            ISystemSettings systemSettings,
            ILogger<NotificationService> logger)
        {
            _systemSettings = systemSettings;
            _logger = logger;
        }

        #endregion Properties And Constructor

        public virtual async Task SendEmailAync(EmailAccount emailAccount, QueuedEmail queuedEmail)
        {
            GuardClausesParameter.Null(emailAccount, nameof(emailAccount));
            GuardClausesParameter.Null(queuedEmail, nameof(queuedEmail));
            try
            {
                List<string> toList = null;
                if (!string.IsNullOrWhiteSpace(queuedEmail.To))
                {
                    toList = queuedEmail.To.Split(new char[] { ',', ';' }).ToList();
                }

                List<string> bccList = null;
                if (!string.IsNullOrWhiteSpace(queuedEmail.Bcc))
                {
                    bccList = queuedEmail.Bcc.Split(new char[] { ',', ';' }).ToList();
                }

                List<string> ccList = null;
                if (!string.IsNullOrWhiteSpace(queuedEmail.Cc))
                {
                    ccList = queuedEmail.Cc.Split(new char[] { ',', ';' }).ToList();
                }

                await CoreUtility.SendEmail(emailAccount.UserName,
                    emailAccount.Password,
                    queuedEmail.Title,
                    queuedEmail.EmailBody,
                    emailAccount.Email,
                    emailAccount.Name, toList,
                    emailAccount.Host,
                    emailAccount.Port,
                    emailAccount.EnableSsl,
                    emailAccount.UseDefaultCredentials,
                    bccList,
                    ccList);

                emailAccount.SendCountByDay++;
                if (emailAccount.SendCountByDay >= emailAccount.DaySendLimit)
                {
                    emailAccount.DaySendLimited = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    emailAccount.SendCountByDay = 0;
                }
                queuedEmail.EmailAccountId = emailAccount.Id;
                queuedEmail.SendTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(100, "SendEmail"), ex, ex.Message);
            }
            finally
            {
                queuedEmail.TrySend++;
            }
        }

        /// <summary>
        /// Get queue email for send email wellcome user
        /// </summary>
        /// <param name="emailTemplate">Template email</param>
        /// <param name="userEmail">Email of user</param>
        /// <param name="userPassowrd">Password of user optional</param>
        /// <param name="userDisplayName">full name of user</param>
        /// <param name="linkCallback">link if have</param>
        /// <returns></returns>
        public virtual QueuedEmail GetQueuedEmailForNewUser(MessageTemplate emailTemplate, string userEmail, string userPassowrd, string userDisplayName, string linkCallback = "")
        {
            GuardClausesParameter.Null(emailTemplate, nameof(emailTemplate));
            GuardClausesParameter.NullOrEmpty(userEmail, nameof(userEmail));
            Dictionary<string, string> replaceValue = new Dictionary<string, string>
                {
                    { "[useremail]", userEmail },
                    { "[userpassword]", userPassowrd },
                    { "[userfullname]", userDisplayName },
                    { "[callbacklink]", linkCallback }
                };
            string title = CoreUtility.ReplaceContentHelper(emailTemplate.Title, replaceValue);
            string content = CoreUtility.ReplaceContentHelper(emailTemplate.Body, replaceValue) + "<br />" + _systemSettings.Common.EmailSignature;
            return new QueuedEmail()
            {
                To = userEmail,
                Title = title,
                EmailBody = content,
                Cc = null,
                Bcc = null,
                ToName = userDisplayName,
                EmailAccountId = null,
                SendTime = null,
                Priority = 5,
                TrySend = 0
            };
        }

        /// <summary>
        /// Get queue email for send email to active account
        /// </summary>
        /// <param name="emailTemplate">Template email</param>
        /// <param name="userEmail">active email user</param>
        /// <param name="webSiteName">system name</param>
        /// <param name="userDisplayName">User full name</param>
        /// <param name="activeLink">Active link</param>
        public virtual QueuedEmail GetQueuedEmailForActiveAccount(MessageTemplate emailTemplate, string userEmail, string webSiteName, string userDisplayName, string activeLink)
        {
            GuardClausesParameter.Null(emailTemplate, nameof(emailTemplate));
            GuardClausesParameter.NullOrEmpty(userEmail, nameof(userEmail));
            GuardClausesParameter.NullOrEmpty(activeLink, nameof(activeLink));
            Dictionary<string, string> replaceValue = new Dictionary<string, string>
                {
                    { "[systemname]", webSiteName },
                    { "[userfullname]", userDisplayName },
                    { "[activelink]", activeLink }
                };
            string title = CoreUtility.ReplaceContentHelper(emailTemplate.Title, replaceValue);
            string content = CoreUtility.ReplaceContentHelper(emailTemplate.Body, replaceValue) + "<br />" + _systemSettings.Common.EmailSignature;
            return new QueuedEmail()
            {
                To = userEmail,
                Title = title,
                EmailBody = content,
                Cc = null,
                Bcc = null,
                ToName = userDisplayName,
                EmailAccountId = null,
                SendTime = null,
                Priority = 5,
                TrySend = 0
            };
        }

        /// <summary>
        /// Get queue email for send email to revovery user passowrd
        /// </summary>
        /// <param name="emailTemplate">Template email</param>
        /// <param name="userEmail">active email user</param>
        /// <param name="webSiteName">system name</param>
        /// <param name="userDisplayName">User full name</param>
        /// <param name="recoveryLink">recovery link</param>
        public virtual QueuedEmail UserPasswordRecovery(MessageTemplate emailTemplate, string userEmail, string webSiteName, string userDisplayName, string recoveryLink)
        {
            GuardClausesParameter.Null(emailTemplate, nameof(emailTemplate));
            GuardClausesParameter.NullOrEmpty(userEmail, nameof(userEmail));
            GuardClausesParameter.NullOrEmpty(recoveryLink, nameof(recoveryLink));
            Dictionary<string, string> replaceValue = new Dictionary<string, string>
                {
                    { "[systemname]", webSiteName },
                    { "[userfullname]", userDisplayName },
                    { "[recoverylink]", recoveryLink }
                };
            string title = CoreUtility.ReplaceContentHelper(emailTemplate.Title, replaceValue);
            string content = CoreUtility.ReplaceContentHelper(emailTemplate.Body, replaceValue) + "<br />" + _systemSettings.Common.EmailSignature;
            return new QueuedEmail()
            {
                To = userEmail,
                Title = title,
                EmailBody = content,
                Cc = null,
                Bcc = null,
                ToName = userDisplayName,
                EmailAccountId = null,
                SendTime = null,
                Priority = 5,
                TrySend = 0
            };
        }
    }
}