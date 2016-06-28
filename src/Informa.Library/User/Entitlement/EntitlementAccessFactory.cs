using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
	[AutowireService]
	public class EntitlementAccessFactory : IEntitlementAccessFactory
	{
		public IEntitlementAccess Create(IEntitlement entitlement, EntitledAccessLevel accessLevel)
		{
			return new EntitlementAccess
			{
				AccessLevel = accessLevel,
				Entitlement = entitlement
			};
		}
	}
}
