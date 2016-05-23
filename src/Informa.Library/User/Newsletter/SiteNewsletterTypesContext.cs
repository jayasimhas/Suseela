using Informa.Library.Publication;
using Informa.Library.Site;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SiteNewsletterTypesContext : ISiteNewsletterTypesContext
	{
		protected readonly ISitePublicationContext SitePublicationContext;
		protected readonly ISiteRootContext SiteRootContext;

		public SiteNewsletterTypesContext(
			ISitePublicationContext sitePublicationContext,
			ISiteRootContext siteRootContext)
		{
			SitePublicationContext = sitePublicationContext;
			SiteRootContext = siteRootContext;
		}

		public ISiteNewsletterTypes Types => new SiteNewsletterTypes
		{
			Breaking = SiteRootContext.Item.Newsletter_Breaking_Type,
			Daily = SiteRootContext.Item.Newsletter_Daily_Type,
			Weekly = SiteRootContext.Item.Newsletter_Weekly_Type,
			Publication = SitePublicationContext.Name		
		};
	}
}
