using Informa.Library.Publication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class SiteNewsletterTypesFactory : ISiteNewsletterTypesFactory
	{
		protected readonly ISitePublicationFactory SitePublicationFactory;

		public SiteNewsletterTypesFactory(
			ISitePublicationFactory sitePublicationFactory)
		{
			SitePublicationFactory = sitePublicationFactory;
		}

		public ISiteNewsletterTypes Create(ISite_Root siteRoot)
		{
			return new SiteNewsletterTypes
			{
				Breaking = siteRoot.Newsletter_Breaking_Type,
				Daily = siteRoot.Newsletter_Daily_Type,
				Weekly = siteRoot.Newsletter_Weekly_Type,
				Publication = SitePublicationFactory.Create(siteRoot)
			};
		}
	}
}
