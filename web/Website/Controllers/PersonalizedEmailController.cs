using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Net;
using System.Text;
using Informa.Library.Site;
using Informa.Library.Mail;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Services.Global;
using Sitecore.Configuration;
using System.Collections.Generic;
using Informa.Library.Utilities.Extensions;
using System;
using System.Web.Configuration;
using log4net;
using Sitecore.Data.Items;
using System.Web;
using Sitecore.Web;
using Sitecore.Resources.Media;
using Informa.Library.Utilities.Settings;

namespace Informa.Web.Controllers
{
    [Route]
    public class PersonalizedEmailController : ApiController
    {
        private readonly EmailUtil _emailUtil;
        private readonly ILog _logger;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteSettings SiteSettings;

        public PersonalizedEmailController(EmailUtil emailUtil, ILog logger, ISiteRootContext siteRootContext, IGlobalSitecoreService globalService, ISiteSettings siteSettings)
        {

            _emailUtil = emailUtil;
            _logger = logger;
            SiteRootContext = siteRootContext;
            GlobalService = globalService;
            SiteSettings = siteSettings;
        }

        [HttpGet]
        public HttpResponseMessage Get(string userId)
        {

            var isMailSent = false;

            var response = new HttpResponseMessage();
            response.StatusCode = string.IsNullOrWhiteSpace(userId) ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
            var emailContent = _emailUtil.CreatePersonalizedEmailBody(userId);
            response.Content = new StringContent(emailContent, Encoding.UTF8, "text/html");

            if (WebConfigurationManager.AppSettings["UnitTest"] != null)
            {

                if (!string.IsNullOrWhiteSpace(userId))
                {


                    var emailBody = GetEmailBody(userId, emailContent.ToString());
                    var personalizedEmail = new Email
                    {
                        To = userId,
                        Subject = "Personalized Test",
                        From = "ebodkhe@sapient.com",
                        Body = emailBody,
                        IsBodyHtml = true


                    };

                    EmailSender EmailSender = new EmailSender();

                    var isEmailSent = EmailSender.Send(personalizedEmail);
                    isMailSent = isEmailSent;
                    if (!isEmailSent)
                    {
                        _logger.Warn($"Email sender failed");
                    }
                }
            }
            return response;

        }


        public string GetEmailBody(string senderEmail, string PersonalizedContent)
        {
            string emailHtml = string.Empty;
            try
            {

                var siteRoot = SiteRootContext.Item;
                var verticalName = siteRoot._Path.Split('/')[3].ToLower();

                //  var footerContent = GlobalService.GetItem<IEmail_Config>(Settings.GetSetting("EmailConfig." + verticalName));

                IHtmlEmailTemplateFactory HtmlEmailTemplateFactory = new HtmlEmailTemplateFactory();
                var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("Emailtemplatepersonalization");


                if (htmlEmailTemplate == null)
                {
                    return null;
                }

                emailHtml = htmlEmailTemplate.Html;
                var replacements = new Dictionary<string, string>
                {
                    ["#FirstName#"] = senderEmail,
                    ["#Personalized_Content#"] = PersonalizedContent.ToString(),
                    ["#email#"] = senderEmail,
                    ["#Date#"] = DateTime.Now.ToString("dddd, d MMM yyyy"),

                    ["#RSS_Link_URL#"] = siteRoot?.RSS_Link.GetLink(),
                    ["#LinkedIn_Link_URL#"] = siteRoot?.LinkedIn_Link.GetLink(),
                    ["#Twitter_Link_URL#"] = siteRoot?.Twitter_Link.GetLink(),


                    ["#TwitterLogo#"] = (siteRoot?.Twitter_Logo != null)? 
                    GetMediaURL(siteRoot.Twitter_Logo.MediaId.ToString())
                        : string.Empty,

                    ["#Logo_URL#"] = (siteRoot?.Email_Logo != null)
                        ? GetMediaURL(siteRoot.Email_Logo.MediaId.ToString())
                        : string.Empty,
                    ["#RssLogo#"] = (siteRoot?.RSS_Logo != null)
                        ? GetMediaURL(siteRoot.RSS_Logo.MediaId.ToString())
                        : string.Empty,
                    ["#LinkedinLogo#"] = (siteRoot?.Linkedin_Logo != null)
                        ? GetMediaURL(siteRoot.Linkedin_Logo.MediaId.ToString())
                        : string.Empty,




                   ["#Environment#"] = SiteSettings.GetSetting("Env.Value", string.Empty),
                













                };

                emailHtml = emailHtml.ReplacePatternCaseInsensitive(replacements);
            }
            catch (Exception ex)
            {
                _logger.Warn($"Not able to replace Content inside body");
            }
            return emailHtml;
        }
        public string GetMediaURL(string mediaId)
        {
            Item imageItem = GlobalService.GetItem<Item>(mediaId);
            if (imageItem == null)
                return string.Empty;

            return $"{HttpContext.Current.Request.Url.Scheme}://{WebUtil.GetHostName()}{MediaManager.GetMediaUrl(imageItem)}";
        }


        public string GetValue(string value, string defaultValue = null)
        {
            return value ?? defaultValue ?? string.Empty;
        }

    }
}
