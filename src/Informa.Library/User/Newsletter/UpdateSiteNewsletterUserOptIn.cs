using Jabberwocky.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class UpdateSiteNewsletterUserOptIn : IUpdateSiteNewsletterUserOptIn
	{
		protected readonly IUpdateNewsletterUserOptIns UpdateOptIns;
		protected readonly ISiteNewsletterUserOptInsContext SiteNewsletterUserOptInsContext;

		public UpdateSiteNewsletterUserOptIn(
			IUpdateNewsletterUserOptIns updateOptIns,
			ISiteNewsletterUserOptInsContext siteNewsletterUserOptInsContext)
		{
			UpdateOptIns = updateOptIns;
			SiteNewsletterUserOptInsContext = siteNewsletterUserOptInsContext;
		}

		public bool Update(string username, bool optIn)
		{
			if (string.IsNullOrWhiteSpace(username))
			{
				return false;
			}

			var optIns = SiteNewsletterUserOptInsContext.OptIns.ToList();

			optIns.ForEach(oi => oi.OptIn = optIn);

			return UpdateOptIns.Update(optIns, username);
		}
	}
}
