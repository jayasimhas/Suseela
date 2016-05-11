using Glass.Mapper.Sc;
using Informa.Library.Mail;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.WebUtils;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Web;
using System;
using System.Globalization;
using System.Web;
using System.Web.Security;

namespace Informa.Library.User.Authentication
{

    [AutowireService(LifetimeScope.Default)]
    public class SendRecoveryPassword : ISendRecoveryPassword
    {

        IEmailSender _emailSender;
        ISiteRootContext _siteRootContext;
        ISitecoreService _sitecoreService;
        public SendRecoveryPassword(IEmailSender emailSender,
            ISiteRootContext siteRootContext,
            ISitecoreService sitecoreService)
        {
            _emailSender = emailSender;
            _siteRootContext = siteRootContext;
            _sitecoreService = sitecoreService;
        }

        public bool SendEmail(string username, string toEmail, string newPassword)
        {
            try
            {
                IPassword_Recovery_Email_Config emailConfigItem = _sitecoreService.GetItem<IPassword_Recovery_Email_Config>(ItemReferences.Instance.PasswordRecoveryEmail);

                string htmlBody = emailConfigItem.Email_Body;
                string from = emailConfigItem.Email_From;
                string subject = emailConfigItem.Email_Subject;
                string to = toEmail;

                //If any of them are empty just return
                if (string.IsNullOrEmpty(htmlBody) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                    return false;

                //replace body tokens with user values
                string messageBody = htmlBody;
                //messageBody = messageBody.Replace("{logo}", UrlUtils.GetMediaURL(_siteRootContext.Item.Email_Logo.MediaId.ToString()));
                messageBody = messageBody.Replace("{username}", username);
                messageBody = messageBody.Replace("{password}", newPassword);

                //Prepare Email variable
                var message = new Email();
                message.Body = messageBody;
                message.From = from;
                message.IsBodyHtml = true;
                message.Subject = string.IsNullOrEmpty(subject) ? "User New Password Notification" : subject;
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
