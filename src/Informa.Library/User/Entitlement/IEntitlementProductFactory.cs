using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;

namespace Informa.Library.User.Entitlement
{
	public interface IEntitlementProductFactory
	{
		IEntitledProduct Create(IEntitled_Product item);
	}
}