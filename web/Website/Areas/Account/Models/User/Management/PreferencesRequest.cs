using Informa.Library.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Informa.Web.Areas.Account.Models.User.Management
{
    public class PreferencesRequest
    {
        public bool NewsletterOptIn { get; set; }
        public bool DoNotSendOffersOptIn { get; set; }

		public string UserName { get; set; }
    }
}