namespace Informa.Library.User.Entitlement
{
	public class EntitlementAccess : IEntitlementAccess
	{
		public EntitledAccessLevel AccessLevel { get; set; }
		public IEntitlement Entitlement { get; set; }
	}
}
