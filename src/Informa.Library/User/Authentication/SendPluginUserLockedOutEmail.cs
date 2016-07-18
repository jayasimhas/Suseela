using Glass.Mapper.Sc;
using Informa.Library.Mail;
using Informa.Library.Site;
using Informa.Library.Utilities.WebUtils;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Globalization;
using System.Web.Security;
using Informa.Library.Wrappers;

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
        }

        public SendPluginUserLockedOutEmail(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public bool SendEmail(MembershipUser user)
        {
            try
            {
                string htmlBody = _dependencies.SiteRootContext.Item.Lockout_Email_Body;
                string from = _dependencies.SiteRootContext.Item.Lockout_Email_From;
                string subject = _dependencies.SiteRootContext.Item.Lockout_Email_Subject;
                string to = _dependencies.SiteRootContext.Item.Lockout_Email_To;

                //If any of them are empty just return
                if (string.IsNullOrEmpty(htmlBody) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                    return false;

                //replace body tokens with user values
                string messageBody = htmlBody;
                messageBody = messageBody.Replace("{logo}", _dependencies.SitecoreUrlWrapper.GetMediaUrl(_dependencies.SiteRootContext.Item.Email_Logo.MediaId));
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
                return _dependencies.EmailSender.Send(message);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
