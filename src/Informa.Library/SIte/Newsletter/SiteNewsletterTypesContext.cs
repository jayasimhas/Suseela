using System.Collections.Generic;
using Informa.Library.Newsletter;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.Site.Newsletter
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteNewsletterTypesContext : ISiteNewsletterTypesContext
	{
		public IEnumerable<NewsletterType> NewsletterTypes
		{
			get
			{
				return new List<NewsletterType>
				{
					{ NewsletterType.Scrip }
				};
			}
		}
	}
}
