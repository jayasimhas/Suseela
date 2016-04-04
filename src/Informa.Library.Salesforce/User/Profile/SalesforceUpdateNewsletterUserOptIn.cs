using Informa.Library.User.Profile;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Newsletter;
using Informa.Library.User;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Profile
{
	public class SalesforceUpdateNewsletterUserOptIn : IUpdateNewsletterUserOptIn
	{
		protected readonly ISalesforceServiceContext Service;
        private readonly IAuthenticatedUserContext UserContext;
        public readonly IQueryNewsletterUserOptIn NewsletterOptIn;

        public SalesforceUpdateNewsletterUserOptIn(
			ISalesforceServiceContext service,
            IAuthenticatedUserContext userContext,
            IQueryNewsletterUserOptIn newsletterOptIn)
		{
			Service = service;
            UserContext = userContext;
            NewsletterOptIn = newsletterOptIn;

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
            if (!UserContext.IsAuthenticated)
                return false;

            var userNewsOptInStatus = NewsletterOptIn.Query(UserContext.User);
            if (userNewsOptInStatus.Success)
            {
                var result = userNewsOptInStatus.NewsletterOptIns.Where(a => a.Name.ToLower().Equals(NewsletterType.Scrip.ToDescriptionString().ToLower()));
                return (result.Any())
                    ? result.First().ReceivesNewsletterAlert
                    : false;
            }
            return false;
		}
	}
}
