namespace Informa.Library.User.Entitlement
{
	public interface IEntitlementAccessFactory
	{
		IEntitlementAccess Create(IEntitlement entitlement, EntitledAccessLevel accessLevel);
	}
}