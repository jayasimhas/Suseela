using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Offer;

namespace Informa.Library.Salesforce.User.Offer
{
    public class SalesforceOfferUserOptedIn : IOfferUserOptedIn
	{
        protected readonly ISalesforceServiceContext Service;

        public SalesforceOfferUserOptedIn(
            ISalesforceServiceContext service)
        {
            Service = service;
        }

        public bool OptedIn(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            var response = Service.Execute(s => s.queryInformationAndOfferOptins(username));

            if (!response.IsSuccess())
            {
                return false;
            }

			return response.doNotSendInformationAndOffersSpecified && !response.doNotSendInformationAndOffers.Value;
		}
    }
}
