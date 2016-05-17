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
		public EntitledProductItemCodeFactory(
			ISiteRootsContext siteRootsContext)
		{
			SiteRootsContext = siteRootsContext;
		}

		public string Create(IEntitled_Product item)
		{
			return SiteRootsContext.SiteRoots.FirstOrDefault(sr => item._Path.StartsWith(sr._Path))?.Publication_Name;
		}
	}
}
