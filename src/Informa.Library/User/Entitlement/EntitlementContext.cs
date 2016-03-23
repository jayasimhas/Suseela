using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class EntitlementContext : IEntitlementContext
    {
		protected readonly IEntitlementAccessLevelContext EntitlementAccessLevelContext;

		public EntitlementContext(
			IEntitlementAccessLevelContext entitlementAccessLevelContext)
        {
			EntitlementAccessLevelContext = entitlementAccessLevelContext;
		}

        #region Implementation of IEntitlementContext

        public bool IsEntitled(IEntitlement entitlement)
        {
			var accessLevel = EntitlementAccessLevelContext.Determine(entitlement);

			switch (accessLevel)
			{
				case EntitledAccessLevel.Free:
				case EntitledAccessLevel.Individual:
				case EntitledAccessLevel.TransparentIP:
					return true;
				default:
					return false;
			}
        }

        #endregion
    }
}