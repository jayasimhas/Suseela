using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Mail;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Article;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Web.Areas.Article.Models.Article.EmailFriend;
using Informa.Library.Utilities.Settings;
using Informa.Library.Utilities.WebApi.Filters;
using log4net;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Web;

namespace Informa.Web.Areas.Article.Controllers
{
	public class EmailFriendController : ApiController
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

		public EmailFriendController(
            ITextTranslator textTranslator,
            ISiteRootContext siteRootContext,
            IEmailFactory emailFactory,
            IEmailSender emailSender,
            IHtmlEmailTemplateFactory htmlEmailTemplateFactory,
            ISiteSettings siteSettings,
            ILog logger,
            IGlobalSitecoreService globalService,
            IArticleSearch articleSearch,
            IArticleService articleService)
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
				_logger.Warn($"Field is null");
				return Ok(new
				{
					success = false
				});
			}

            //var emailFrom = GetValue(siteRoot?.Email_From_Address);
            var emailFrom = string.Format("{0} <{1}>", siteRoot.Publication_Name, GetValue(siteRoot?.Email_From_Address));
			var allEmails = request.RecipientEmail.Split(';');
			var result = true;
			var emailBody = GetEmailBody(request.SenderEmail, request.SenderName,
					request.ArticleNumber, request.PersonalMessage);

			foreach (var eachEmail in allEmails)
			{
				string specificEmailBody = emailBody
					.ReplacePatternCaseInsensitive("#friend_name#", eachEmail)
					.ReplacePatternCaseInsensitive("#RECIPIENT_EMAIL#", eachEmail);

				var friendEmail = new Email
				{
					To = eachEmail,
					Subject = request.ArticleTitle,
					From = emailFrom,
					Body = specificEmailBody,
					IsBodyHtml = true
				};

				var isEmailSent = EmailSender.Send(friendEmail);
				if (!isEmailSent)
				{
					_logger.Warn($"Email sender failed");
					result = false;
				}
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
				var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("EmailFriend");

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

				// Article Body
				var article = GetArticle(articleNumber);
				replacements["#article_date#"] = article?.Actual_Publish_Date.ToString("dd MMMM yyyy") ?? string.Empty;
				replacements["#article_mediatype#"] = article?.Media_Type?.Item_Name ?? string.Empty;
				replacements["#article_title#"] = article?.Title ?? String.Empty;
				replacements["#article_titleURL#"] = (article != null)
					? $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}{article._Url}?utm_medium=email&utm_campaign=emailfriend&utm_source={siteRoot.Publication_Name}&utm_content={article._Id}"
					: string.Empty;
				replacements["#article_authorBy#"] = (article != null && article.Authors.Any())
					? TextTranslator.Translate("Article.By")
					: string.Empty;
				replacements["#article_author#"] = (article != null && article.Authors.Any())
					? string.Join(",", article.Authors.Select(a => $"{a.First_Name} {a.Last_Name}"))
					: string.Empty;
				replacements["#article_summary#"] = (article != null && !string.IsNullOrEmpty(article.Summary))
                    ? ArticleService.GetArticleSummary(article)
                    : string.Empty;

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

		#region Email Results


		[HttpPost]
        [ValidateReasons]
        [ArgumentsRequired]
        public IHttpActionResult EmailSearch(EmailFriendSearchRequest request)
		{
			var siteRoot = SiteRootContext.Item;

			if (string.IsNullOrWhiteSpace(request.RecipientEmail)
				|| string.IsNullOrWhiteSpace(request.SenderEmail)
				|| string.IsNullOrWhiteSpace(request.SenderName)
				|| string.IsNullOrWhiteSpace(request.PersonalMessage)
				|| string.IsNullOrEmpty(request.ResultIDs)
				|| string.IsNullOrEmpty(request.QueryTerm))
			{
				return Ok(new
				{
					success = false
				});
			}

            var emailFrom = string.Format("{0} <{1}>", siteRoot.Publication_Name, GetValue(siteRoot?.Email_From_Address));
			var allEmails = request.RecipientEmail.Split(';');
			var result = true;
			var emailBody = GetEmailSearchBody(
					request.SenderEmail,
					request.SenderName,
					request.ResultIDs.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries),
					request.PersonalMessage,
					request.QueryTerm,
					request.QueryUrl);

			foreach (var eachEmail in allEmails)
			{
				string specificEmailBody = emailBody
					.ReplacePatternCaseInsensitive("#friend_name#", eachEmail)
					.ReplacePatternCaseInsensitive("#RECIPIENT_EMAIL#", eachEmail);

				var friendEmail = new Email
				{
					To = eachEmail,
					Subject = request.Subject,
					From = emailFrom,
					Body = specificEmailBody,
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

		public string GetEmailSearchBody(string senderEmail, string senderName, IEnumerable<string> resultIDs, string message, string queryTerm, string queryUrl)
		{
			string emailHtml = string.Empty;
			try
			{
				var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("_BaseEmail");
				var searchTemplate = HtmlEmailTemplateFactory.Create("EmailFriendSearch");
				var searchRow = HtmlEmailTemplateFactory.Create("EmailFriendSearchRow");
				var searchAuthor = HtmlEmailTemplateFactory.Create("EmailFriendSearchAuthor");

				if (htmlEmailTemplate == null
					|| searchRow == null
					|| searchTemplate == null
					|| searchAuthor == null)
				{
					return null;
				}

				//main email information
				var siteRoot = SiteRootContext.Item;
				emailHtml = htmlEmailTemplate.Html.Replace("#Body_Content#", searchTemplate.Html);
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
					["#query_url#"] = queryUrl,
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
					["#notice_message#"] = TextTranslator.Translate("Search.EmailPopout.Notice"),
					["#search_query#"] = queryTerm,
					["#see_more#"] = TextTranslator.Translate("Search.SeeMore"),
                    ["#sender_name_message#"] = !string.IsNullOrEmpty(message)
                        ? TextTranslator.Translate("Search.Message").Replace("#SENDER_NAME#", senderName)
                        : string.Empty
                };

				//search results
				StringBuilder resultBody = new StringBuilder();
				int j = 0;
				foreach (string id in resultIDs)
				{
					Guid g;
					if (!Guid.TryParse(id, out g))
						continue;

					var result = GlobalService.GetItem<I___BasePage>(g);
					if (result == null)
						continue;

					//article authors
					var article = GlobalService.GetItem<IArticle>(g);
					bool hasAuthors = article != null && article.Authors.Any();
					string byline = TextTranslator.Translate("Article.By");
					string authorInsert = (hasAuthors)
						? searchAuthor.Html
							.ReplacePatternCaseInsensitive("#article_authorBy#", (!string.IsNullOrEmpty(byline)) ? byline : string.Empty)
							.ReplacePatternCaseInsensitive("#article_author#", string.Join(",", article.Authors.Select(a => $"{a.First_Name} {a.Last_Name}")))
						: string.Empty;

					var row = searchRow.Html.Replace("#result_publication#", SiteRootContext?.Item?.Publication_Name)
						.ReplacePatternCaseInsensitive("#result_title#", result.Title)
						.ReplacePatternCaseInsensitive("#result_summary#", SearchSummaryUtil.GetTruncatedSearchSummary(result.Body))
						.ReplacePatternCaseInsensitive("#result_url#", $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}{result._Url}")
						.ReplacePatternCaseInsensitive("#author_insert#", authorInsert);

					resultBody.Append(row);
					j++;
				}
				replacements["#result_count#"] = j.ToString();
				replacements["#result_list#"] = resultBody.ToString();

				emailHtml = emailHtml.ReplacePatternCaseInsensitive(replacements);
			}
			catch (Exception ex)
			{

			}
			return emailHtml;
		}

		#endregion Email Results

	    protected IArticle GetArticle(string articleNumber)
	    {
            IArticleSearchFilter filter = ArticleSearch.CreateFilter();
            filter.ArticleNumbers = articleNumber.SingleToList();
            return ArticleSearch.Search(filter).Articles.FirstOrDefault();
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
