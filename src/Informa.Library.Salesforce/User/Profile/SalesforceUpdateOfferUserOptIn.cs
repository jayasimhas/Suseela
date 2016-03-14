﻿using Informa.Library.User.Profile;
using Informa.Library.User;
using Informa.Library.Salesforce.EBIWebServices;

namespace Informa.Library.Salesforce.User.Profile
{
	public class SalesforceUpdateOfferUserOptIn : IUpdateOfferUserOptIn
	{
		protected readonly ISalesforceServiceContext Service;
		 
		public SalesforceUpdateOfferUserOptIn(
			ISalesforceServiceContext service)
		{
			Service = service;
		}

		public bool Update(string userName, bool optIn)
		{
			if (string.IsNullOrEmpty(userName))
			{
				return false;
			}

			var response = Service.Execute(s => s.updateDoNotSendInformationAndOffers(userName, !optIn));

			return response.IsSuccess();
		}
	}
}
