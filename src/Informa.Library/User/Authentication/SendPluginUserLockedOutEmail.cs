using Glass.Mapper.Sc;
using Informa.Library.Mail;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.WebUtils;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Globalization;
using System.Web.Security;

namespace Informa.Library.User.Authentication
{
    [AutowireService(LifetimeScope.Default)]
    public class SendUserLockedOutEmail : ISendUserLockedOutEmail
    {
        IEmailSender _emailSender;
        ISiteRootContext _siteRootContext;
        ISitecoreService _sitecoreService;
        public SendUserLockedOutEmail(IEmailSender emailSender,
            ISiteRootContext siteRootContext,
            ISitecoreService sitecoreService)
        {
            _emailSender = emailSender;
            _siteRootContext = siteRootContext;
            _sitecoreService = sitecoreService;
        }

        public bool SendEmail(MembershipUser user)
        {
            try
            {
                IUser_Lockout_Emails_Config emailConfigItem = _sitecoreService.GetItem<IUser_Lockout_Emails_Config>(ItemReferences.Instance.UserLockoutedEmails);

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
                    messageBody = messageBody.Replace("{logo}", UrlUtils.GetMediaURL(_siteRootContext.Item.Email_Logo.MediaId.ToString()));
                }
                catch
                {
                    messageBody = messageBody.Replace("{logo}", string.Empty);
                }
                messageBody = messageBody.Replace("{username}", user.UserName);
                messageBody = messageBody.Replace("{dateLockedOut}", user.LastLockoutDate.ToString(CultureInfo.InvariantCulture));

                //Prepare Email variable
                var message = new Email();
                message.Body = messageBody;
                message.From = from;
                message.IsBodyHtml = true;
                message.Subject = string.IsNullOrEmpty(subject) ? "User Lockout Notification" : subject;
                message.To = to;

                //Send email
                return _emailSender.Send(message);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.ToString(), this);
                return false;
            }
        }
    }
}
