using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class UserIpAddressEntitlementsContext : IUserIpAddressEntitlementsContext
    { 
        protected readonly IUserIpAddressContext UserIpAddressContext;
        protected readonly IUserSession UserSession;
        protected readonly IGetIPEntitlements GetIpEntitlements;
		protected readonly IDefaultEntitlementsFactory DefaultEntitlementsFactory;

		public UserIpAddressEntitlementsContext(
			IUserIpAddressContext userIpAddressContext,
			IUserSession userSession,
			IGetIPEntitlements getIpEntitlements,
			IDefaultEntitlementsFactory defaultEntitlementsFactory)
        {
            UserIpAddressContext = userIpAddressContext;
            UserSession = userSession;
            GetIpEntitlements = getIpEntitlements;
			DefaultEntitlementsFactory = defaultEntitlementsFactory;
		}

        private const string EntitlementSessionKeyPrefix = nameof(UserIpAddressEntitlementsContext);

		public string EntitlementSessionKey => $"{EntitlementSessionKeyPrefix}{UserIpAddressContext.IpAddress}";

		public IEnumerable<IEntitlement> Entitlements
        {
            get
            {
				var entitlementsSession = UserSession.Get<IEnumerable<IEntitlement>>(EntitlementSessionKey);
				var entitlements = entitlementsSession.HasValue ? entitlementsSession.Value : new List<IEntitlement>();

                if (entitlements.Any())
				{
					return entitlements;
				}
                    
                entitlements = GetIpEntitlements.GetEntitlements(UserIpAddressContext.IpAddress);

                if(!entitlements.Any())
				{
					entitlements = DefaultEntitlementsFactory.Create().ToList();
				}

                Entitlements = entitlements;

                return entitlements;
            }
			set { UserSession.Set(EntitlementSessionKey, value); }
        }

		public void RefreshEntitlements()
        {
			UserSession.Clear(EntitlementSessionKey);
		}

		public EntitledAccessLevel GetProductAccessLevel(string productCode)
		{
			return Entitlements.Any(e => e.ProductCode == productCode) ? EntitledAccessLevel.TransparentIP : EntitledAccessLevel.UnEntitled;
		}
	}
}