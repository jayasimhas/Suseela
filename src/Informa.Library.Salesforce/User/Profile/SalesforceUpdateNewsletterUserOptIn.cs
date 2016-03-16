using Informa.Library.User.Profile;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Newsletter;
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

		public bool Update(IEnumerable<INewsletterUserOptIn> newsletterOptIns, string userName)
		{
			if (string.IsNullOrEmpty(userName))
			{
				return false;
			}

			var optIns = newsletterOptIns.Select(noi => new EBI_EmailNewsLetterOptin
			{
				optinName = noi.NewsletterType.ToDescriptionString(),
				IsReceivingEmailNewsletter = noi.OptIn,
				IsReceivingEmailNewsletterSpecified = true
			}).ToArray();

			var response = Service.Execute(s => s.updateEmailNewsletterOptIns(userName, optIns));
			return response.IsSuccess();
		}

		public bool IsUserSignedUp(string userName)
		{
			if (!Sitecore.Context.User.IsAuthenticated)
			{
				return false;
			}
			var response = Service.Execute(s => s.queryEmailNewsletterOptins(userName));
			if (response.emailNewsletterOptins != null)
			{
				var optionSignup = response.emailNewsletterOptins.Where(x => (x !=null && x.optinName.Equals(NewsletterType.Scrip.ToDescriptionString()))).Count();
				if (optionSignup > 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}
