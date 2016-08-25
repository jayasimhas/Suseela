using Glass.Mapper.Sc;
using Informa.Library.Mail;
using Informa.Library.Site;
using Informa.Library.Utilities.WebUtils;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Globalization;
using System.Web.Security;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Utilities.References;

namespace Informa.Library.User.Authentication
{
    [AutowireService]
    public class SendPluginUserLockedOutEmail : ISendUserLockedOutEmail
    {
        private readonly IDependencies _dependencies;

        [AutowireService(true)]
        public interface IDependencies
        {
            IEmailSender EmailSender { get; }
            ISiteRootContext SiteRootContext { get; }
            ISitecoreUrlWrapper SitecoreUrlWrapper { get; }
            ISitecoreService SitecoreService { get; }
        }

        public SendPluginUserLockedOutEmail(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public bool SendEmail(MembershipUser user)
        {
            try
            {
                IUser_Lockout_Emails_Config emailConfigItem = _dependencies.SitecoreService.GetItem<IUser_Lockout_Emails_Config>(ItemReferences.Instance.UserLockoutedEmails);

                string htmlBody = emailConfigItem.Lockout_Email_Body;
                string from = emailConfigItem.Lockout_Email_From;
                string subject = emailConfigItem.Lockout_Email_Subject;
                string to = emailConfigItem.Lockout_Email_To;

                //If any of them are empty just return
                if (string.IsNullOrEmpty(htmlBody) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                    return false;

                //replace body tokens with user values
                string messageBody = htmlBody;

                try
                {
                    messageBody = messageBody.Replace("{logo}", _dependencies.SitecoreUrlWrapper.GetMediaUrl(_dependencies.SiteRootContext.Item.Email_Logo.MediaId));
                }
                catch
                {
                    messageBody = messageBody.Replace("{logo}", string.Empty);
                }

                messageBody = messageBody.Replace("{username}", user.UserName);
                messageBody = messageBody.Replace("{dateLockedOut}", DateTime.Now.ToString());


                //Prepare Email variable
                var message = new Email();
                message.Body = messageBody;
                message.From = from;
                message.IsBodyHtml = true;
                message.Subject = string.IsNullOrEmpty(subject) ? "User Lockout Notification" : subject;
                message.To = to;

                //Send email
                return _dependencies.EmailSender.Send(message);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.ToString(), this);
                return false;
            }
        }
    }
}
