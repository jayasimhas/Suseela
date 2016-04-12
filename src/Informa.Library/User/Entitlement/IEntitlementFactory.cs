using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.User.Entitlement
{
	public interface IEntitlementFactory
	{
		IEntitlement Create(IEntitledProductItem item);
	}
}