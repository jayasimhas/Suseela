using Informa.Library.Publication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Newsletter
{
	[AutowireService]
	public class SiteNewsletterTypesFactory : ISiteNewsletterTypesFactory
	{
		protected readonly ISitePublicationCodeFactory SitePublicationCodeFactory;

		public SiteNewsletterTypesFactory(
			ISitePublicationCodeFactory sitePublicationCodeFactory)
		{
			SitePublicationCodeFactory = sitePublicationCodeFactory;
		}

		public ISiteNewsletterTypes Create(ISite_Root siteRoot)
		{
			return new SiteNewsletterTypes
			{
				Breaking = siteRoot.Newsletter_Breaking_Type,
				Daily = siteRoot.Newsletter_Daily_Type,
				Weekly = siteRoot.Newsletter_Weekly_Type,
				Publication = SitePublicationCodeFactory.Create(siteRoot)
			};
		}
	}
}
