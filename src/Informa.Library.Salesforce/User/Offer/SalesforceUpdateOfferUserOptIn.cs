using Informa.Library.User.Offer;
using Informa.Library.Salesforce.EBIWebServices;
using log4net;

namespace Informa.Library.Salesforce.User.Offer
{
	public class SalesforceUpdateOfferUserOptIn : IUpdateOfferUserOptIn
	{
		protected readonly ISalesforceServiceContext Service;
        protected readonly ILog Logger;

        public SalesforceUpdateOfferUserOptIn(
			ISalesforceServiceContext service,
             ILog logger)
		{
			Service = service;
            Logger = logger;
		}

		public bool Update(string userName, bool optIn)
		{
            Logger.Error("UserName : " + userName);
            if (string.IsNullOrEmpty(userName))
			{
				return false;
			}

			var response = Service.Execute(s => s.updateDoNotSendInformationAndOffers(userName, !optIn));

			return response.IsSuccess();
		}
	}
}
