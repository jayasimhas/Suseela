using System.Collections.Generic;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class PreferencesRequest
	{
		public string[] NewsletterOptIns { get; set; }
        public bool DoNotSendOffersOptIn { get; set; }
    }
}