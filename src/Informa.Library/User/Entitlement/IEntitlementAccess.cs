namespace Informa.Library.User.Entitlement
{
	public interface IEntitlementAccess
	{
		IEntitlement Entitlement { get; }
		EntitledAccessLevel AccessLevel { get; }
	}
}
