using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement.Scrip
{
	[AutowireService(LifetimeScope.Default)]
	public class ScripEntitlementFactory : IEntitlementFactory
	{
		public IEntitlement Create(IEntitledProductItem item)
		{
			return new ScripEntitlement
			{
				ProductCode = "SCRIP"
			};
		}
	}
}