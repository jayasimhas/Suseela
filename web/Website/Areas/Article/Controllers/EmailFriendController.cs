using System;
using System.Collections.Generic;
using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Mail;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Web.Areas.Article.Models.Article.EmailFriend;
using Informa.Web.Controllers;

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

		public EmailFriendController(ArticleUtil articleUtil, ITextTranslator textTranslator, ISiteRootContext siteRootContext, IEmailFactory emailFactory,
			Func<string, ISitecoreService> sitecoreFactory, IEmailSender emailSender, IHtmlEmailTemplateFactory htmlEmailTemplateFactory)
		{
			EmailSender = emailSender;
			_articleUtil = articleUtil;
			_service = sitecoreFactory(Constants.MasterDb);
			HtmlEmailTemplateFactory = htmlEmailTemplateFactory;
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
			EmailFactory = emailFactory;
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

			var emailBody = GetEmailBody(request.SenderEmail, request.SenderName,
				request.ArticleNumber, "Aakash Shah", request.RecipientEmail, request.PersonalMessage);
			Email friendEmail = new Email
			{
				To = request.RecipientEmail,
				Subject = request.ArticleTitle,
				From = emailFrom,
				Body = emailBody,
				IsBodyHtml = true
			};

			var result = EmailSender.Send(friendEmail);

			return Ok(new
			{
				success = result
			});
		}

		public string GetEmailBody(string senderEmail, string senderName, string articleNumber, string friendName, string friendEmail, string message)
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
				var replacements = new Dictionary<string, string>();

				replacements["#Logo_URL#"] = GetValue(siteRoot?.Email_Logo?.Src);
				replacements["#Date#"] = DateTime.Now.ToString("dddd, d MMMM yyyy");
				replacements["#RSS_Link_URL#"] = siteRoot?.RSS_Link.GetLink();
				replacements["#RSS_Link_Text#"] = GetValue(siteRoot?.RSS_Link?.Text);
				replacements["#LinkedIn_Link_URL#"] = siteRoot?.LinkedIn_Link.GetLink();
				replacements["#LinkedIn_Link_Text#"] = GetValue(siteRoot?.LinkedIn_Link?.Text);
				replacements["#Twitter_Link_URL#"] = siteRoot?.Twitter_Link.GetLink();
				replacements["#Twitter_Link_Text#"] = GetValue(siteRoot?.Twitter_Link?.Text);

				//replacements["#Body_Content#"] = GetValue(siteRoot?.Email_A_Friend_Body_Content)
				//	.ReplaceCaseInsensitive("#reciever_name#", friendName)
				//	.ReplaceCaseInsensitive("#sender_name#", senderEmail)
				//	.ReplaceCaseInsensitive("#sender_email#", senderName)
				//	.ReplaceCaseInsensitive("#personal_message#", message);

				// Article Body
				var article = _articleUtil.GetArticleByNumber(articleNumber);
				if (article != null)
				{
					replacements["#article_date#"] = article.Actual_Publish_Date.ToShortDateString();
					replacements["#article_mediaType#"] = article.Media_Type != null ? article.Media_Type.Item_Name : string.Empty;
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

				replacements["#Footer_Content#"] = GetValue(siteRoot?.Email_A_Friend_Footer_Content)
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
	}
}
