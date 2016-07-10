using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.Newsletter;
using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Newsletter
{
    public class SalesforceFindNewsletterUserOptIns : IFindNewsletterUserOptIns
    {
		protected readonly INewsletterUserOptInFactory OptInFactory;
		protected readonly ISalesforceServiceContext Service;

        public SalesforceFindNewsletterUserOptIns(
			INewsletterUserOptInFactory optInFactory,
            ISalesforceServiceContext service)
        {
			OptInFactory = optInFactory;
            Service = service;
        }

        public IEnumerable<INewsletterUserOptIn> Find(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return Enumerable.Empty<INewsletterUserOptIn>();
            }
            
            var response = Service.Execute(s => s.queryEmailNewsletterOptins(username));

            if (!response.IsSuccess())
            {
                return Enumerable.Empty<INewsletterUserOptIn>();
            }

			var optIns = response.emailNewsletterOptins.Select(eno => OptInFactory.Create(
				eno.optinName,
				eno.IsReceivingEmailNewsletterSpecified && eno.IsReceivingEmailNewsletter.Value)
			);

			return optIns;
		}
    }
}
