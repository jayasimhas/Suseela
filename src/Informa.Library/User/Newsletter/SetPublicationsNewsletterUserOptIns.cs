using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SetPublicationsNewsletterUserOptIns : ISetPublicationsNewsletterUserOptIns
	{
		protected readonly ISetByTypeNewsletterUserOptInsContext SetNewsletterOptIns;
		protected readonly ISitesNewsletterTypes SitesNewsletterTypes;

		public SetPublicationsNewsletterUserOptIns(
			ISetByTypeNewsletterUserOptInsContext setNewsletterOptIns,
			ISitesNewsletterTypes sitesNewsletterTypes)
		{
			SetNewsletterOptIns = setNewsletterOptIns;
			SitesNewsletterTypes = sitesNewsletterTypes;
		}

		public bool Set(IEnumerable<string> publications)
		{
			var newsletterTypes = new List<string>();

			foreach (var siteTypes in SitesNewsletterTypes.SiteTypes.Where(st => publications.Any(p => string.Equals(p, st.Publication.Code, System.StringComparison.InvariantCultureIgnoreCase))))
			{
				AddType(siteTypes.Breaking, ref newsletterTypes);
				AddType(siteTypes.Daily, ref newsletterTypes);
				AddType(siteTypes.Weekly, ref newsletterTypes);
			}

			return SetNewsletterOptIns.Set(newsletterTypes);
		}

		public void AddType(string newsletterType, ref List<string> newsletterTypes)
		{
			if (!string.IsNullOrEmpty(newsletterType))
			{
				newsletterTypes.Add(newsletterType);
			}
		}
	}
}
