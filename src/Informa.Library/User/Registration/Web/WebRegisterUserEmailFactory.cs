using Informa.Library.Mail;
using System.Collections.Generic;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.Utilities.Settings;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.PerScope)]
	public class WebRegisterUserEmailFactory : IWebRegisterUserEmailFactory
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IBaseHtmlEmailFactory EmailFactory;
		protected readonly ISiteSettings SiteSettings;

		public WebRegisterUserEmailFactory(
			ISiteRootContext siteRootContext,
			IBaseHtmlEmailFactory emailFactory,
			ISiteSettings siteSettings)
		{
			SiteRootContext = siteRootContext;
			EmailFactory = emailFactory;
			SiteSettings = siteSettings;
		}

		public IEmail Create(INewUser newUser)
		{
			if (newUser == null)
			{
				return null;
			}

			var email = EmailFactory.Create();

			if (email == null)
			{
				return null;
			}

			var siteRoot = SiteRootContext.Item;
			var emailTo = newUser.Username;
			var replacements = new Dictionary<string, string>();

			replacements["#Email#"] = emailTo;
			replacements["#Envrionment#"] = SiteSettings.GetSetting("Env.Value", string.Empty);
			replacements["#Page_Type#"] = "registration-successful-email-in-inbox";
			replacements["#Body_Content#"] = GetValue(siteRoot?.Registration_Email_Body)
				.ReplacePatternCaseInsensitive("#First_Name#", newUser.FirstName)
				.ReplacePatternCaseInsensitive("#Last_Name#", newUser.LastName);

			email.Body = email.Body.ReplacePatternCaseInsensitive(replacements);

			var emailSubject = GetValue(siteRoot?.Registration_Email_Subject);

			email.To = emailTo;
			email.Subject = emailSubject;

			return email;
		}

		public string GetValue(string value, string defaultValue = null)
		{
			return value ?? defaultValue ?? string.Empty;
		}
	}
}
