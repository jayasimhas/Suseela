using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Mail;
using Informa.Library.Services.Article;
using Informa.Library.Services.Captcha;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Utilities.Settings;
using log4net;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Web;
using Informa.Web.Areas.Article.Models.Article.AskAnalyst;

namespace Informa.Web.Areas.Article.Controllers
{
	public class AskTheAnalystController : ApiController
	{
		protected readonly IEmailSender EmailSender;
		protected readonly IHtmlEmailTemplateFactory HtmlEmailTemplateFactory;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IEmailFactory EmailFactory;
		protected readonly ISiteSettings SiteSettings;
		private readonly ILog _logger;
		protected readonly IGlobalSitecoreService GlobalService;
		private readonly IBaseHtmlEmailFactory BaseEmailFactory;
		protected readonly IArticleSearch ArticleSearch;
		protected readonly IArticleService ArticleService;
		protected readonly IRecaptchaService RecaptchaService;

		public AskTheAnalystController(
						ITextTranslator textTranslator,
						ISiteRootContext siteRootContext,
						IEmailFactory emailFactory,
						IEmailSender emailSender,
						IHtmlEmailTemplateFactory htmlEmailTemplateFactory,
						ISiteSettings siteSettings,
						ILog logger,
						IGlobalSitecoreService globalService,
						IArticleSearch articleSearch,
						IArticleService articleService,
						IRecaptchaService recaptchaService)
		{
			EmailSender = emailSender;
			HtmlEmailTemplateFactory = htmlEmailTemplateFactory;
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
			EmailFactory = emailFactory;
			SiteSettings = siteSettings;
			_logger = logger;
			GlobalService = globalService;
			ArticleSearch = articleSearch;
			ArticleService = articleService;
			RecaptchaService = recaptchaService;
		}

        [HttpPost]
        public IHttpActionResult EmailToAnalyst(EmailAnalyst request)
        {
            var siteRoot = SiteRootContext.Item;

            // VERIFY CAPTCHA
            if (!RecaptchaService.Verify(request.RecaptchaResponse))
            {
                return Ok(new
                {
                    success = false
                });
            }

            if (string.IsNullOrWhiteSpace(request.AskTheAnalystEmail)
                    || string.IsNullOrWhiteSpace(request.SenderEmail)
                    || string.IsNullOrWhiteSpace(request.SenderName)
                    || string.IsNullOrWhiteSpace(request.ArticleNumber)
                    || string.IsNullOrWhiteSpace(request.CompanyName)
                    || string.IsNullOrWhiteSpace(request.PhoneNumber)
                    || string.IsNullOrWhiteSpace(request.PublicationName))
            {
                _logger.Warn($"Field is null");
                return Ok(new
                {
                    success = false
                });
            }

            var allEmails = request.AskTheAnalystEmail;// request.RecipientEmail.Split(';');
            var result = true;
            var emailBody = GetEmailBody(request.SenderEmail, request.SenderName,
                            request.ArticleNumber, request.PersonalQuestion);
            string specificEmailBody = emailBody
                        .ReplacePatternCaseInsensitive("#friend_name#", request.AskTheAnalystEmail)
                        .ReplacePatternCaseInsensitive("#RECIPIENT_EMAIL#", request.AskTheAnalystEmail)
                        .ReplacePatternCaseInsensitive("#publication name#",request.PublicationName);            
				var analystEmail = new Email
				{
					To = request.AskTheAnalystEmail,
					Subject = request.ArticleTitle,
					From = request.SenderEmail,
					Body = specificEmailBody,                    
					IsBodyHtml = true
				};

				var isEmailSent = EmailSender.Send(analystEmail);
				if (!isEmailSent)
				{
					_logger.Warn($"Email sender failed");
					result = false;
				}			
			return Ok(new
			{
				success = result
			});
		}

		public string GetEmailBody(string senderEmail, string senderName, string articleNumber, string message)
		{
			string emailHtml = string.Empty;
			try
			{
				var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("EmailAnalyst");

				if (htmlEmailTemplate == null)
				{
					return null;
				}

				var siteRoot = SiteRootContext.Item;
				emailHtml = htmlEmailTemplate.Html;
				var footerContent = GlobalService.GetItem<IEmail_Config>(Constants.ScripEmailConfig);
				var replacements = new Dictionary<string, string>
				{
					["#Environment#"] = SiteSettings.GetSetting("Env.Value", string.Empty),
					["#Date#"] = DateTime.Now.ToString("dddd, d MMMM yyyy"),
					["#RSS_Link_URL#"] = siteRoot?.RSS_Link.GetLink(),
					["#LinkedIn_Link_URL#"] = siteRoot?.LinkedIn_Link.GetLink(),
					["#Twitter_Link_URL#"] = siteRoot?.Twitter_Link.GetLink(),
					["#sender_name#"] = senderName,
					["#sender_email#"] = senderEmail,
					["#Logo_URL#"] = (siteRoot?.Email_Logo != null)
								? GetMediaURL(siteRoot.Email_Logo.MediaId.ToString())
								: string.Empty,
					["#RssLogo#"] = (siteRoot?.RSS_Logo != null)
								? GetMediaURL(siteRoot.RSS_Logo.MediaId.ToString())
								: string.Empty,
					["#LinkedinLogo#"] = (siteRoot?.Linkedin_Logo != null)
								? GetMediaURL(siteRoot.Linkedin_Logo.MediaId.ToString())
								: string.Empty,
					["#TwitterLogo#"] = (siteRoot?.Twitter_Logo != null)
								? GetMediaURL(siteRoot.Twitter_Logo.MediaId.ToString())
								: string.Empty,
					["#personal_message#"] = (!string.IsNullOrEmpty(message))
								? $"\"{message}\""
								: string.Empty,
					["#Footer_Content#"] = GetValue(footerContent?.Email_A_Friend_Footer_Content)
								.ReplacePatternCaseInsensitive("#SENDER_EMAIL#", senderEmail),
					["#sender_name_message#"] = !string.IsNullOrEmpty(message)
								? TextTranslator.Translate("Search.Message").Replace("#SENDER_NAME#", senderName)
								: string.Empty
				};				
				emailHtml = emailHtml.ReplacePatternCaseInsensitive(replacements);
			}
			catch (Exception ex)
			{
				_logger.Warn($"Email failed to send: {senderEmail}:{senderName}:{articleNumber}:{message}", ex);
			}
			return emailHtml;
		}

		public string GetValue(string value, string defaultValue = null)
		{
			return value ?? defaultValue ?? string.Empty;
		}      

		public string GetMediaURL(string mediaId)
		{
			Item imageItem = GlobalService.GetItem<Item>(mediaId);
			if (imageItem == null)
				return string.Empty;

			return $"{HttpContext.Current.Request.Url.Scheme}://{WebUtil.GetHostName()}{MediaManager.GetMediaUrl(imageItem)}";
		}
	}
}
