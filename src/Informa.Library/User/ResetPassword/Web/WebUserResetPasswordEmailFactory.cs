using Informa.Library.Globalization;
using Informa.Library.Mail;
using Informa.Library.Site;
using Informa.Library.User.Profile;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebUserResetPasswordEmailFactory : IWebUserResetPasswordEmailFactory
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IFindUserProfileByUsername FindUserProfile;
		protected readonly IWebUserResetPasswordUrlFactory UrlFactory;
		protected readonly IHtmlEmailTemplateFactory HtmlEmailTemplateFactory;
		protected readonly IEmailFactory EmailFactory;

		public WebUserResetPasswordEmailFactory(
			ITextTranslator textTranslator,
			ISiteRootContext siteRootContext,
			IFindUserProfileByUsername findUserProfile,
			IWebUserResetPasswordUrlFactory urlFactory,
			IHtmlEmailTemplateFactory htmlEmailTemplateFactory,
			IEmailFactory emailFactory)
		{
			TextTranslator = textTranslator;
			SiteRootContext = siteRootContext;
			FindUserProfile = findUserProfile;
			UrlFactory = urlFactory;
			HtmlEmailTemplateFactory = htmlEmailTemplateFactory;
			EmailFactory = emailFactory;
		}

		public IEmail Create(IUserResetPassword userResetPassword)
		{
			if (userResetPassword == null)
			{
				return null;
			}

			var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("ResetPassword");

			if (htmlEmailTemplate == null)
			{
				return null;
			}

			var siteRoot = SiteRootContext.Item;
			var emailHtml = htmlEmailTemplate.Html;
			var userProfile = FindUserProfile.Find(userResetPassword.Username);
			var emailTo = userResetPassword.Username;			
			var replacements = new Dictionary<string, string>();

			replacements["#Logo_URL#"] = GetValue(siteRoot?.Email_Logo?.Src);
			replacements["#Date#"] = DateTime.Now.ToString("dddd, d MMMM yyyy");
			replacements["#RSS_Link_URL#"] = siteRoot?.RSS_Link.GetLink();
			replacements["#RSS_Link_Text#"] = GetValue(siteRoot?.RSS_Link?.Text);
			replacements["#LinkedIn_Link_URL#"] = siteRoot?.LinkedIn_Link.GetLink();
			replacements["#LinkedIn_Link_Text#"] = GetValue(siteRoot?.LinkedIn_Link?.Text);
			replacements["#Twitter_Link_URL#"] = siteRoot?.Twitter_Link.GetLink();
			replacements["#Twitter_Link_Text#"] = GetValue(siteRoot?.Twitter_Link?.Text);
			replacements["#Reset_Link_URL#"] = UrlFactory.Create(userResetPassword);
			replacements["#Reset_Link_Text#"] = GetValue(siteRoot?.Reset_Password_Email_Link_Text, TextTranslator.Translate("Authentication.ResetPassword.Email.ResetLink"));
			replacements["#Body_Content#"] = GetValue(siteRoot?.Reset_Password_Email_Body);
			replacements["#Footer_Content#"] = GetValue(siteRoot?.Reset_Password_Email_Footer).ReplaceCaseInsensitive("#Email#", emailTo);

			if (userProfile != null)
			{
				replacements["#First_Name#"] = userProfile.FirstName;
				replacements["#Last_Name#"] = userProfile.LastName;
			}

			emailHtml = emailHtml.ReplaceCaseInsensitive(replacements);

			var email = EmailFactory.Create();
			var emailSubject = GetValue(siteRoot?.Reset_Password_Email_Subject);
			var emailFrom = GetValue(siteRoot?.Email_From_Address);

			email.To = emailTo;
			email.From = emailFrom;
			email.Body = emailHtml;
			email.IsBodyHtml = true;
			email.Subject = emailSubject;

			return email;
		}

		public string GetValue(string value, string defaultValue = null)
		{
			return value ?? defaultValue ?? string.Empty;
		}
	}
}
