using Informa.Library.Site;
using System;
using System.Collections.Generic;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.WebUtils;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Mail
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class BaseHtmlEmailFactory : IBaseHtmlEmailFactory
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IHtmlEmailTemplateFactory HtmlEmailTemplateFactory;
		protected readonly IEmailFactory EmailFactory;

		public BaseHtmlEmailFactory(
			ISiteRootContext siteRootContext,
			IHtmlEmailTemplateFactory htmlEmailTemplateFactory,
			IEmailFactory emailFactory)
		{
			SiteRootContext = siteRootContext;
			HtmlEmailTemplateFactory = htmlEmailTemplateFactory;
			EmailFactory = emailFactory;
		}

		public IEmail Create()
		{
			var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("_BaseEmail");

			if (htmlEmailTemplate == null)
			{
				return null;
			}

			var siteRoot = SiteRootContext.Item;
			var emailHtml = htmlEmailTemplate.Html;
			var replacements = new Dictionary<string, string>();
			if (siteRoot?.Email_Logo != null)
			{
				replacements["#Logo_URL#"] = UrlUtils.GetMediaURL(siteRoot.Email_Logo.MediaId.ToString());
			}
			//replacements["#Logo_URL#"] = GetValue(siteRoot?.Email_Logo?.Src);
			replacements["#Date#"] = DateTime.Now.ToString("dddd, d MMMM yyyy");
			replacements["#RSS_Link_URL#"] = siteRoot?.RSS_Link.GetLink();
			replacements["#RSS_Link_Text#"] = GetValue(siteRoot?.RSS_Link?.Text);
			replacements["#LinkedIn_Link_URL#"] = siteRoot?.LinkedIn_Link.GetLink();
			replacements["#LinkedIn_Link_Text#"] = GetValue(siteRoot?.LinkedIn_Link?.Text);
			replacements["#Twitter_Link_URL#"] = siteRoot?.Twitter_Link.GetLink();
			replacements["#Twitter_Link_Text#"] = GetValue(siteRoot?.Twitter_Link?.Text);
			replacements["#Footer_Content#"] = GetValue(siteRoot?.Email_Footer);

			emailHtml = emailHtml.ReplacePatternCaseInsensitive(replacements);

			var email = EmailFactory.Create();
			var emailFrom = GetValue(siteRoot?.Email_From_Address);

			email.From = emailFrom;
			email.Body = emailHtml;
			email.IsBodyHtml = true;

			return email;
		}

		public string GetValue(string value, string defaultValue = null)
		{
			return value ?? defaultValue ?? string.Empty;
		}
	}
}
