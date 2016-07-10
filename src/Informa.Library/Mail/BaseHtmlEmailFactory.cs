using Informa.Library.Site;
using System;
using System.Collections.Generic;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Web;

namespace Informa.Library.Mail
{
	[AutowireService(LifetimeScope.PerScope)]
	public class BaseHtmlEmailFactory : IBaseHtmlEmailFactory
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IHtmlEmailTemplateFactory HtmlEmailTemplateFactory;
		protected readonly IEmailFactory EmailFactory;
	    protected readonly IGlobalSitecoreService GlobalService;


        public BaseHtmlEmailFactory(
			ISiteRootContext siteRootContext,
			IHtmlEmailTemplateFactory htmlEmailTemplateFactory,
			IEmailFactory emailFactory,
            IGlobalSitecoreService globalService)
		{
			SiteRootContext = siteRootContext;
			HtmlEmailTemplateFactory = htmlEmailTemplateFactory;
			EmailFactory = emailFactory;
            GlobalService = globalService;
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
				replacements["#Logo_URL#"] = GetMediaURL(siteRoot.Email_Logo.MediaId.ToString());
			}

			replacements["#Date#"] = DateTime.Now.ToString("dddd, d MMMM yyyy");
			replacements["#RSS_Link_URL#"] = siteRoot?.RSS_Link.GetLink();
			if (siteRoot?.RSS_Logo != null)
			{
				replacements["#RssLogo#"] = GetMediaURL(siteRoot.RSS_Logo.MediaId.ToString());
			}
			
			replacements["#LinkedIn_Link_URL#"] = siteRoot?.LinkedIn_Link.GetLink();
			if (siteRoot?.Linkedin_Logo != null)
			{
				replacements["#LinkedinLogo#"] = GetMediaURL(siteRoot.Linkedin_Logo.MediaId.ToString());
			}

			replacements["#Twitter_Link_URL#"] = siteRoot?.Twitter_Link.GetLink();
			if (siteRoot?.Twitter_Logo != null)
			{
				replacements["#TwitterLogo#"] = GetMediaURL(siteRoot.Twitter_Logo.MediaId.ToString());
			}

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

        public string GetMediaURL(string mediaId)
        {
            Item imageItem = GlobalService.GetItem<Item>(mediaId);
            if (imageItem == null)
                return string.Empty;
            
            return $"{HttpContext.Current.Request.Url.Scheme}://{WebUtil.GetHostName()}{MediaManager.GetMediaUrl(imageItem)}";
        }
    }
}
