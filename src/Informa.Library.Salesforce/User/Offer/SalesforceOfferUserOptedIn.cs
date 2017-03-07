using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Newsletter;
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

        public OffersOptIn OptedIn(string username)
        {
            var optinsList = new OffersOptIn();
            if (string.IsNullOrEmpty(username))
            {
                optinsList.OptIn = false;
                return optinsList;
            }
            var response = Service.Execute(s => s.queryInformationAndOfferOptins(username));

            if (!response.IsSuccess())
            {
                optinsList.OptIn = false;
                return optinsList;
            }
            if (response.doNotSendInformationAndOffersSpecified && !response.doNotSendInformationAndOffers.Value)
            {
                optinsList.OptIn = true;
                return optinsList;
            }
            return optinsList;
        }
    }
}
