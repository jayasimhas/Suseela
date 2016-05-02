﻿using Informa.Library.User.Profile;
using System;

namespace Informa.Library.Salesforce.User.Profile
{
	public class SalesforceSubscription : ISubscription
	{
		public string DocumentID { get; set; }
		public string Publication { get; set; }
		public string SubscriptionType { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string ProductCode { get; set; }
		public string ProductGuid { get; set; }
		public string ProductType { get; set; }
	}
}
