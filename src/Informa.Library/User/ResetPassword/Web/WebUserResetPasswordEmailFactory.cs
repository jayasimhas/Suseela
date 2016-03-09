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
		protected readonly IBaseHtmlEmailFactory EmailFactory;

		public WebUserResetPasswordEmailFactory(
			ITextTranslator textTranslator,
			ISiteRootContext siteRootContext,
			IFindUserProfileByUsername findUserProfile,
			IWebUserResetPasswordUrlFactory urlFactory,
			IHtmlEmailTemplateFactory htmlEmailTemplateFactory,
			IBaseHtmlEmailFactory emailFactory)
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

			var email = EmailFactory.Create();
			var siteRoot = SiteRootContext.Item;
			var resetPasswordHtml = htmlEmailTemplate.Html;
			var userProfile = FindUserProfile.Find(userResetPassword.Username);
			var emailTo = userResetPassword.Username;
			var resetPasswordReplacements = new Dictionary<string, string>();

			resetPasswordReplacements["#Reset_Link_URL#"] = UrlFactory.Create(userResetPassword);
			resetPasswordReplacements["#Reset_Link_Text#"] = GetValue(siteRoot?.Reset_Password_Email_Link_Text, TextTranslator.Translate("Authentication.ResetPassword.Email.ResetLink"));
			resetPasswordReplacements["#Body_Content#"] = GetValue(siteRoot?.Reset_Password_Email_Body);

			resetPasswordHtml = resetPasswordHtml.ReplaceCaseInsensitive(resetPasswordReplacements);

			var baseReplacements = new Dictionary<string, string>();

			baseReplacements["#Body_Content#"] = resetPasswordHtml;
			baseReplacements["#Email#"] = emailTo;

			if (userProfile != null)
			{
				baseReplacements["#First_Name#"] = userProfile.FirstName;
				baseReplacements["#Last_Name#"] = userProfile.LastName;
			}

			email.Body = email.Body.ReplaceCaseInsensitive(baseReplacements);
			email.To = emailTo;
			email.Subject = GetValue(siteRoot?.Reset_Password_Email_Subject);

			return email;
		}

		public string GetValue(string value, string defaultValue = null)
		{
			return value ?? defaultValue ?? string.Empty;
		}
	}
}
