using Informa.Library.Publication;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Jabberwocky.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService]
	public class EntitledProductItemCodeFactory : IEntitledProductItemCodeFactory
	{
		protected readonly ISiteRootsContext SiteRootsContext;
		protected readonly ISitePublicationCodeFactory PublicationCodeFactory;

		public EntitledProductItemCodeFactory(
			ISiteRootsContext siteRootsContext,
			ISitePublicationCodeFactory publicationCodeFactory)
		{
			SiteRootsContext = siteRootsContext;
			PublicationCodeFactory = publicationCodeFactory;
		}

		public string Create(IEntitled_Product item)
		{
			var siteRoot = SiteRootsContext.SiteRoots.FirstOrDefault(sr => item._Path.StartsWith(sr._Path));

			return PublicationCodeFactory.Create(siteRoot);
		}
	}
}
