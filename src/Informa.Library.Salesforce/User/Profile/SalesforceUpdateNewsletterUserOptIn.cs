﻿using Informa.Library.User.Profile;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.User;
using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Profile
{
	public class SalesforceUpdateNewsletterUserOptIn : IUpdateNewsletterUserOptIn
	{
		protected readonly ISalesforceServiceContext Service;

		public SalesforceUpdateNewsletterUserOptIn(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public bool Update(IUser user, IEnumerable<INewsletterUserOptIn> newsletterOptIns)
		{
			if (string.IsNullOrEmpty(user?.Username))
			{
				return false;
			}

			var optIns = newsletterOptIns.Select(noi => new EBI_EmailNewsLetterOptin
			{
				optinName = noi.NewsletterType.ToString(),
				IsReceivingEmailNewsletter = noi.OptIn,
				IsReceivingEmailNewsletterSpecified = true
			}).ToArray();

			var response = Service.Execute(s => s.updateEmailNewsletterOptIns(user.Username, optIns));

			return response.IsSuccess();
		}
	}
}
