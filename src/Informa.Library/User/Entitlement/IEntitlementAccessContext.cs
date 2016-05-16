namespace Informa.Library.User.Entitlement
{
	public interface IEntitlementAccessContext
	{
		IEntitlementAccess Create(string productCode);
	}
}