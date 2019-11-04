using Pl.Core.Entities;
using System.Threading.Tasks;

namespace Pl.Core.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="emailAccount">Email account for sending</param>
        /// <param name="queuedEmail">Email content for send</param>
        Task SendEmailAync(EmailAccount emailAccount, QueuedEmail queuedEmail);

        /// <summary>
        /// Get queue email for send email wellcome user
        /// </summary>
        /// <param name="emailTemplate">Template email</param>
        /// <param name="userEmail">Email of user</param>
        /// <param name="userPassowrd">Password of user optional</param>
        /// <param name="userDisplayName">full name of user</param>
        /// <param name="linkCallback">link if have</param>
        /// <returns></returns>
        QueuedEmail GetQueuedEmailForNewUser(MessageTemplate emailTemplate, string userEmail, string userPassowrd, string userDisplayName, string linkCallback = "");

        /// <summary>
        /// Get queue email for send email to active account
        /// </summary>
        /// <param name="emailTemplate">Template email</param>
        /// <param name="userEmail">active email user</param>
        /// <param name="webSiteName">system name</param>
        /// <param name="userDisplayName">User full name</param>
        /// <param name="activeLink">Active link</param>
        QueuedEmail GetQueuedEmailForActiveAccount(MessageTemplate emailTemplate, string userEmail, string webSiteName, string userDisplayName, string activeLink);

        /// <summary>
        /// Get queue email for send email to revovery user passowrd
        /// </summary>
        /// <param name="emailTemplate">Template email</param>
        /// <param name="userEmail">active email user</param>
        /// <param name="webSiteName">system name</param>
        /// <param name="userDisplayName">User full name</param>
        /// <param name="recoveryLink">recovery link</param>
        QueuedEmail UserPasswordRecovery(MessageTemplate emailTemplate, string userEmail, string webSiteName, string userDisplayName, string recoveryLink);
    }
}