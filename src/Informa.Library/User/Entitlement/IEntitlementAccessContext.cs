namespace Informa.Library.User.Entitlement
{
	public interface IEntitlementAccessContext
	{
		IEntitlementAccess Find(IEntitledProduct entitledProduct);
	}
}