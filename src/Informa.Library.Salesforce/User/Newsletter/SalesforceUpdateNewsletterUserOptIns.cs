using Informa.Library.User.Newsletter;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Newsletter;
using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Newsletter
{
	public class SalesforceUpdateNewsletterUserOptIns : IUpdateNewsletterUserOptIns
	{
		protected readonly ISalesforceServiceContext Service;

        public SalesforceUpdateNewsletterUserOptIns(
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
	}
}
