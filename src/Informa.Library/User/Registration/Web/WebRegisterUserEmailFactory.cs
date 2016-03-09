﻿using Informa.Library.Mail;
using System.Collections.Generic;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebRegisterUserEmailFactory : IWebRegisterUserEmailFactory
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IBaseHtmlEmailFactory EmailFactory;

		public WebRegisterUserEmailFactory(
			ISiteRootContext siteRootContext,
			IBaseHtmlEmailFactory emailFactory)
		{
			SiteRootContext = siteRootContext;
			EmailFactory = emailFactory;
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
			replacements["#Body_Content#"] = GetValue(siteRoot?.Registration_Email_Body)
				.ReplaceCaseInsensitive("#First_Name#", newUser.FirstName)
				.ReplaceCaseInsensitive("#Last_Name#", newUser.LastName);

			email.Body = email.Body.ReplaceCaseInsensitive(replacements);

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
