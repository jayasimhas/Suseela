using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Mail;
using Informa.Library.Search.Utilities;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.WebUtils;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Web.Areas.Article.Models.Article.EmailFriend;
using Informa.Web.Controllers;
using Informa.Library.Utilities.Settings;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Web.Areas.Article.Controllers
{
	public class EmailFriendController : ApiController
	{
		private ISitecoreService _service;
		private ArticleUtil _articleUtil;
		protected readonly IEmailSender EmailSender;
		protected readonly IHtmlEmailTemplateFactory HtmlEmailTemplateFactory;
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IEmailFactory EmailFactory;
		protected readonly ISiteSettings SiteSettings;
	    protected readonly ISitecoreService SitecoreService;

		public EmailFriendController(ArticleUtil articleUtil, ITextTranslator textTranslator, ISiteRootContext siteRootContext, IEmailFactory emailFactory,
			Func<string, ISitecoreService> sitecoreFactory, IEmailSender emailSender, IHtmlEmailTemplateFactory htmlEmailTemplateFactory, ISiteSettings siteSettings,
            ISitecoreService sitecoreService)
		{
			EmailSender = emailSender;
			_articleUtil = articleUtil;
			_service = sitecoreFactory(Constants.MasterDb);
			HtmlEmailTemplateFactory = htmlEmailTemplateFactory;
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
			EmailFactory = emailFactory;
			SiteSettings = siteSettings;
		    SitecoreService = sitecoreService;
		}

		[HttpPost]
		public IHttpActionResult Email(EmailFriendRequest request)
		{
			var siteRoot = SiteRootContext.Item;

			if (string.IsNullOrWhiteSpace(request.RecipientEmail)
				|| string.IsNullOrWhiteSpace(request.SenderEmail)
				|| string.IsNullOrWhiteSpace(request.SenderName)
				|| string.IsNullOrWhiteSpace(request.ArticleNumber))
			{
				return Ok(new
				{
					success = false
				});
			}

			var emailFrom = GetValue(siteRoot?.Email_From_Address);
			var allEmails = request.RecipientEmail.Split(';');
			var result = true;
			foreach (var eachEmail in allEmails)
			{
				var emailBody = GetEmailBody(request.SenderEmail, request.SenderName,
					request.ArticleNumber, eachEmail, request.PersonalMessage);
				var friendEmail = new Email
				{
					To = eachEmail,
					Subject = request.ArticleTitle,
					From = emailFrom,
					Body = emailBody,
					IsBodyHtml = true
				};

				var isEmailSent = EmailSender.Send(friendEmail);
				if (!isEmailSent)
				{
					result = false;
				}
			}
			return Ok(new
			{
				success = result
			});
		}

		public string GetEmailBody(string senderEmail, string senderName, string articleNumber, string friendEmail, string message)
		{
			string emailHtml = string.Empty;
			try
			{
				var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("EmailFriend");

				if (htmlEmailTemplate == null)
				{
					return null;
				}

				var siteRoot = SiteRootContext.Item;
				emailHtml = htmlEmailTemplate.Html;
				var replacements = new Dictionary<string, string>
				{
					["#Envrionment#"] = SiteSettings.GetSetting("Env.Value", string.Empty),
					["#Date#"] = DateTime.Now.ToString("dddd, d MMMM yyyy"),
					["#RSS_Link_URL#"] = siteRoot?.RSS_Link.GetLink(),					
					["#LinkedIn_Link_URL#"] = siteRoot?.LinkedIn_Link.GetLink(),					
					["#Twitter_Link_URL#"] = siteRoot?.Twitter_Link.GetLink(),					
					["#friend_name#"] = friendEmail,
					["#sender_name#"] = senderName,
					["#sender_email#"] = senderEmail
				};

				if (siteRoot?.Email_Logo != null)
				{
					replacements["#Logo_URL#"] = UrlUtils.GetMediaURL(siteRoot.Email_Logo.MediaId.ToString());
				}
				
				if (siteRoot?.RSS_Logo != null)
				{
					replacements["#RssLogo#"] = UrlUtils.GetMediaURL(siteRoot.RSS_Logo.MediaId.ToString());
				}
				
				if (siteRoot?.Linkedin_Logo != null)
				{
					replacements["#LinkedinLogo#"] = UrlUtils.GetMediaURL(siteRoot.Linkedin_Logo.MediaId.ToString());
				}
				
				if (siteRoot?.Twitter_Logo != null)
				{
					replacements["#TwitterLogo#"] = UrlUtils.GetMediaURL(siteRoot.Twitter_Logo.MediaId.ToString());
				}

				if (!string.IsNullOrEmpty(message))
				{
					replacements["#personal_message#"] = '"' + message + '"';
				}
				
				// Article Body
				var article = _articleUtil.GetArticleByNumber(articleNumber);
				if (article != null)
				{
					replacements["#article_date#"] = article.Actual_Publish_Date.ToString("dd MMMM yyyy");
					replacements["#article_mediatype#"] = article.Media_Type != null ? article.Media_Type.Item_Name : string.Empty;
					replacements["#article_title#"] = article.Title;
					replacements["#article_titleURL#"] = article._Url;

					var authorString = string.Empty;
					foreach (var eachAuthor in article.Authors)
					{
						authorString = eachAuthor.First_Name + eachAuthor.Last_Name + ",";
					}
					if (!string.IsNullOrEmpty(authorString))
					{
						replacements["#article_authorBy#"] = "By";
						replacements["#article_author#"] = authorString;
					}
					else
					{
						replacements["#article_authorBy#"] = string.Empty;
						replacements["#article_author#"] = authorString;
					}

					replacements["#article_summary#"] = article.Summary;
				}

				var footerContent = _service.GetItem<IEmail_Config>(Constants.ScripEmailConfig);				
				replacements["#Footer_Content#"] = GetValue(footerContent?.Email_A_Friend_Footer_Content)
					.ReplacePatternCaseInsensitive("#SENDER_EMAIL#", senderEmail)
					.ReplacePatternCaseInsensitive("#RECIPIENT_EMAIL#", friendEmail);


				emailHtml = emailHtml.ReplacePatternCaseInsensitive(replacements);
			}
			catch (Exception ex)
			{
				
			}
			return emailHtml;
		}

	    public string GetValue(string value, string defaultValue = null)
		{
			return value ?? defaultValue ?? string.Empty;
		}

        #region Email Results


        [HttpPost]
        [HttpGet]
        public IHttpActionResult EmailSearch(string RecipientEmail, string SenderEmail, string SenderName, string PersonalMessage, string ResultIDs, string QueryTerm, string Subject)
        {
            var siteRoot = SiteRootContext.Item;

            if (string.IsNullOrWhiteSpace(RecipientEmail)
                || string.IsNullOrWhiteSpace(SenderEmail)
                || string.IsNullOrWhiteSpace(SenderName)
                || string.IsNullOrWhiteSpace(PersonalMessage)
                || string.IsNullOrEmpty(ResultIDs)
                || string.IsNullOrEmpty(QueryTerm))
            {
                return Ok(new
                {
                    success = false
                });
            }

            var emailFrom = GetValue(siteRoot?.Email_From_Address);
            var allEmails = RecipientEmail.Split(';');
            var result = true;
            foreach (var eachEmail in allEmails)
            {
                var emailBody = GetEmailSearchBody(
                    SenderEmail, 
                    SenderName,
                    ResultIDs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries), 
                    eachEmail, 
                    PersonalMessage, 
                    QueryTerm);

                var friendEmail = new Email
                {
                    To = eachEmail,
                    Subject = Subject,
                    From = emailFrom,
                    Body = emailBody,
                    IsBodyHtml = true
                };

                var isEmailSent = EmailSender.Send(friendEmail);
                if (!isEmailSent)
                {
                    result = false;
                }
            }
            return Ok(new
            {
                success = result
            });
        }

        public string GetEmailSearchBody(string senderEmail, string senderName, IEnumerable<string> resultIDs, string friendEmail, string message, string queryTerm)
        {
            string emailHtml = string.Empty;
            try
            {
                var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("EmailFriendSearch");
                var resultRow = HtmlEmailTemplateFactory.Create("EmailFriendSearchRow");

                if (htmlEmailTemplate == null || resultRow == null)
                {
                    return null;
                }

                var siteRoot = SiteRootContext.Item;
                emailHtml = htmlEmailTemplate.Html;
                var replacements = new Dictionary<string, string>
                {
                    ["#Envrionment#"] = SiteSettings.GetSetting("Env.Value", string.Empty),
                    ["#Date#"] = DateTime.Now.ToString("dddd, d MMMM yyyy"),
                    ["#RSS_Link_URL#"] = siteRoot?.RSS_Link.GetLink(),
                    ["#LinkedIn_Link_URL#"] = siteRoot?.LinkedIn_Link.GetLink(),
                    ["#Twitter_Link_URL#"] = siteRoot?.Twitter_Link.GetLink(),
                    ["#friend_name#"] = friendEmail,
                    ["#sender_name#"] = senderName,
                    ["#sender_email#"] = senderEmail
                };

                if (siteRoot?.Email_Logo != null)
                {
                    replacements["#Logo_URL#"] = UrlUtils.GetMediaURL(siteRoot.Email_Logo.MediaId.ToString());
                }

                if (siteRoot?.RSS_Logo != null)
                {
                    replacements["#RssLogo#"] = UrlUtils.GetMediaURL(siteRoot.RSS_Logo.MediaId.ToString());
                }

                if (siteRoot?.Linkedin_Logo != null)
                {
                    replacements["#LinkedinLogo#"] = UrlUtils.GetMediaURL(siteRoot.Linkedin_Logo.MediaId.ToString());
                }

                if (siteRoot?.Twitter_Logo != null)
                {
                    replacements["#TwitterLogo#"] = UrlUtils.GetMediaURL(siteRoot.Twitter_Logo.MediaId.ToString());
                }
                if (!string.IsNullOrEmpty(message))
                {
                    replacements["#personal_message#"] = '"' + message + '"';
                }
                
                var footerContent = _service.GetItem<IEmail_Config>(Constants.ScripEmailConfig);
                replacements["#Footer_Content#"] = GetValue(footerContent?.Email_A_Friend_Footer_Content)
                    .ReplacePatternCaseInsensitive("#SENDER_EMAIL#", senderEmail)
                    .ReplacePatternCaseInsensitive("#RECIPIENT_EMAIL#", friendEmail);

                replacements["#notice_message#"] = TextTranslator.Translate("Search.EmailPopout.Notice");
                replacements["#search_query#"] = queryTerm;

                StringBuilder resultBody = new StringBuilder();
                int j = 0;
                foreach (string id in resultIDs)
                {
                    Guid g;
                    if(!Guid.TryParse(id, out g))
                        continue;

                    var result = SitecoreService.GetItem<I___BasePage>(g);
                    if (result == null)
                        continue;

                    var row = resultRow.Html.Replace("#result_publication#", SiteRootContext?.Item?.Publication_Name)
                        .Replace("#result_title#", result.Title)
                        .Replace("#result_summary#", SearchSummaryUtil.GetTruncatedSearchSummary(result.Body));
                    
                    resultBody.Append(row);
                    j++;
                }
                replacements["#result_count# "] = j.ToString();
                replacements["#result_list#"] = resultBody.ToString();

                emailHtml = emailHtml.ReplacePatternCaseInsensitive(replacements);
            }
            catch (Exception ex)
            {

            }
            return emailHtml;
        }

        #endregion Email Results
    }
}
