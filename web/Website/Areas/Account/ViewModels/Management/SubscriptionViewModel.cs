using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class SubscriptionViewModel
	{
		public string Publication { get; set; }
		public string Type { get; set; }
		public DateTime Expiration { get; set; }
		public bool Renewable { get; set; }
		public bool Subscribable { get; set; }
	}
}