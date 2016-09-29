using System.Collections.Generic;
using System.Web.Http;
using Informa.Library.Globalization;
using Informa.Library.Mail;
using Informa.Library.Services.Captcha;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Utilities.Settings;
using log4net;
using Informa.Web.Areas.DCD.Models;
using Jabberwocky.Autofac.Attributes;
using Informa.Library.Authors;
using Informa.Model.DCD;
using Informa.Library.Utilities.Parsers;
using Informa.Models.DCD;
using Informa.Library.Wrappers;
using Informa.Library.DCD;

namespace Informa.Web.Areas.DCD.Controllers
{
    public class EmailDCDController : ApiController {

        private readonly IDependencies _;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies {
            IEmailSender EmailSender { get; set; }
            IBaseHtmlEmailFactory baseEmailFactory { get; set; }
            IHtmlEmailTemplateFactory HtmlEmailTemplateFactory { get; set; }
            ITextTranslator TextTranslator { get; set; }
            ISiteRootContext SiteRootContext { get; set; }
            ISiteSettings SiteSettings { get; set; }
            //ILog _logger { get; set; }
            IGlobalSitecoreService GlobalService { get; set; }
            IBaseHtmlEmailFactory BaseEmailFactory { get; set; }
            IRecaptchaService RecaptchaService { get; set; }
            IDealService DealService { get; set; }
            ICompanyService CompanyService { get; set; }
        }

        public EmailDCDController(IDependencies dependencies) {
            _ = dependencies;
        }

        [HttpPost]
        public IHttpActionResult EmailFriendCompany(EmailCompanyRequest request) {

            if (!_.RecaptchaService.Verify(request.RecaptchaResponse))
                return Ok(new { success = false });

            var siteRoot = _.SiteRootContext.Item;

            if (string.IsNullOrWhiteSpace(request.RecipientEmail)
                    || string.IsNullOrWhiteSpace(request.SenderEmail)
                    || string.IsNullOrWhiteSpace(request.SenderName)
                    || string.IsNullOrWhiteSpace(request.Subject)) {
                //_._logger.Warn($"Field is null");
                return Ok(new { success = false });
            }

            var emailFrom = string.Format("{0} <{1}>", siteRoot.Publication_Name, _.BaseEmailFactory.GetValue(siteRoot?.Email_From_Address));
            var allEmails = request.RecipientEmail.Split(';');
            var result = true;
            IEmail email = GetEmailBody(request.SenderEmail, request.SenderName, _.CompanyService.GetCompanyUrl(request.CompanyId), _.CompanyService.GetCompanyName(request.CompanyId), _.CompanyService.GetCompanyDescription(request.CompanyId), request.PersonalMessage, "company");

            foreach (var eachEmail in allEmails) {
                if (!SendEmail(eachEmail, emailFrom, request.Subject, email))
                    result = false;
            }

            return Ok(new { success = result });
        }

        [HttpPost]
        public IHttpActionResult EmailFriendDeal(EmailDealRequest request) {

            if (!_.RecaptchaService.Verify(request.RecaptchaResponse))
                return Ok(new { success = false });

            var siteRoot = _.SiteRootContext.Item;

            if (string.IsNullOrWhiteSpace(request.RecipientEmail)
                    || string.IsNullOrWhiteSpace(request.SenderEmail)
                    || string.IsNullOrWhiteSpace(request.SenderName)
                    || string.IsNullOrWhiteSpace(request.Subject)) {
                //_._logger.Warn($"Field is null");
                return Ok(new { success = false });
            }

            var emailFrom = string.Format("{0} <{1}>", siteRoot.Publication_Name, _.BaseEmailFactory.GetValue(siteRoot?.Email_From_Address));
            var allEmails = request.RecipientEmail.Split(';');
            var result = true;
            
            IEmail email = GetEmailBody(request.SenderEmail, request.SenderName, _.DealService.GetDealUrl(request.DealId), _.DealService.GetDealTitle(request.DealId), _.DealService.GetDealSummary(request.DealId), request.PersonalMessage, "deal");

            foreach (var eachEmail in allEmails) {
                if (!SendEmail(eachEmail, emailFrom, request.Subject, email))
                    result = false;
            }

            return Ok(new { success = result });
        }

        public IEmail GetEmailBody(string senderEmail, string senderName, string dcdUrl, string dcdTitle, string dcdSummary, string message, string emailType) {
            var replacements = new Dictionary<string, string> {
                ["#Body_Content#"] = _.HtmlEmailTemplateFactory.Create("EmailFriendDCD")?.Html ?? string.Empty,
                ["#Footer_Content#"] = _.BaseEmailFactory
                    .GetValue(_.GlobalService.GetItem<IEmail_Config>(Constants.ScripEmailConfig)?.Email_A_Friend_Footer_Content)
                    .ReplacePatternCaseInsensitive("#SENDER_EMAIL#", senderEmail),
                ["#email_type#"] = emailType,
                ["#sender_name#"] = senderName,
                ["#sender_email#"] = senderEmail,
                ["#personal_message#"] = (!string.IsNullOrEmpty(message))
                    ? $"\"{message}\""
                    : string.Empty,
                ["#sender_name_message#"] = !string.IsNullOrEmpty(message)
                    ? _.TextTranslator.Translate("Search.Message").Replace("#SENDER_NAME#", senderName)
                    : string.Empty,
                ["#dcd_URL#"] = dcdUrl,
                ["#dcd_title#"] = dcdTitle,
                ["#dcd_summary#"] = dcdSummary
            };

            IEmail e = _.BaseEmailFactory.Create(replacements);

            return e;
        }

        private bool SendEmail(string toEmail, string fromEmail, string subject, IEmail email) {

            string specificEmailBody = email.Body
                        .ReplacePatternCaseInsensitive("#friend_name#", toEmail)
                        .ReplacePatternCaseInsensitive("#RECIPIENT_EMAIL#", toEmail);

            var friendEmail = new Email {
                To = toEmail,
                Subject = subject,
                From = fromEmail,
                Body = specificEmailBody,
                IsBodyHtml = true
            };

            var isEmailSent = _.EmailSender.Send(friendEmail);
            if (isEmailSent)
                return true;

            //_._logger.Warn($"Email failed to send. To:{toEmail}, From:{fromEmail}, subject:{subject}");

            return false;
        }
    }
}

