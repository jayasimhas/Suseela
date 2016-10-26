using System;
using System.Collections.Generic;
using System.Web;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Web;
using Informa.Library.Utilities.Settings;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Library.Mail
{
	[AutowireService(LifetimeScope.PerScope)]
	public class BaseHtmlEmailFactory : IBaseHtmlEmailFactory
	{
		protected ISitecoreContext SitecoreContext;
		protected readonly IHtmlEmailTemplateFactory HtmlEmailTemplateFactory;
		protected readonly IEmailFactory EmailFactory;
	    protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteSettings SiteSettings;


        public BaseHtmlEmailFactory(
			ISitecoreContext sitecoreContext,
			IHtmlEmailTemplateFactory htmlEmailTemplateFactory,
			IEmailFactory emailFactory,
            IGlobalSitecoreService globalService,
            ISiteSettings siteSettings)
		{
			SitecoreContext = sitecoreContext;
			HtmlEmailTemplateFactory = htmlEmailTemplateFactory;
			EmailFactory = emailFactory;
            GlobalService = globalService;
            SiteSettings = siteSettings;
		}

        public IEmail Create() {
            return Create(new Dictionary<string, string>());
        }
        
        public IEmail Create(Dictionary<string, string> replacements)
		{
			var siteRootContext = SitecoreContext?.GetRootItem<ISite_Root>();

			var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("_BaseEmail");

			if (htmlEmailTemplate == null)
                return null;
		
			var siteRoot = siteRootContext;
			var emailHtml = htmlEmailTemplate.Html;

            replacements.SetValue("#Environment#", SiteSettings.GetSetting("Env.Value", string.Empty));
            replacements.SetValue("#Logo_URL#", (siteRoot?.Email_Logo != null)
                    ? GetMediaURL(siteRoot.Email_Logo.MediaId.ToString())
                    : string.Empty);
            replacements.SetValue("#Date#", DateTime.Now.ToString("dddd, d MMMM yyyy"));
            replacements.SetValue("#RSS_Link_URL#", siteRoot?.RSS_Link.GetLink());
            replacements.SetValue("#RssLogo#", (siteRoot?.RSS_Logo != null)
                    ? GetMediaURL(siteRoot.RSS_Logo.MediaId.ToString())
                    : string.Empty);
            replacements.SetValue("#LinkedIn_Link_URL#", siteRoot?.LinkedIn_Link.GetLink());
            replacements.SetValue("#LinkedinLogo#", (siteRoot?.Linkedin_Logo != null)
                    ? GetMediaURL(siteRoot.Linkedin_Logo.MediaId.ToString())
                    : string.Empty);
            replacements.SetValue("#Twitter_Link_URL#", siteRoot?.Twitter_Link.GetLink());
            replacements.SetValue("#TwitterLogo#", (siteRoot?.Twitter_Logo != null)
                    ? GetMediaURL(siteRoot.Twitter_Logo.MediaId.ToString())
                    : string.Empty);
            replacements.SetValue("#Footer_Content#", GetValue(siteRoot?.Email_Footer));
            
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

    public static class DictionaryExtensions {
        public static void SetValue(this Dictionary<string, string> replacements, string key, string value) {
            if (replacements.ContainsKey(key))
                return;
            replacements.Add(key, value);
        }
    }
}
