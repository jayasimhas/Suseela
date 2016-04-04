using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class AuthenticatedIpContext : IAuthenticatedIPContext
    { 
        protected readonly IUserIpAddressContext UserIpAddressContext;
        protected readonly IUserSession UserSession;
        protected readonly IGetIPEntitlements GetIpEntitlements;

        public AuthenticatedIpContext(IUserIpAddressContext userIpAddressContext, IUserSession userSession, IGetIPEntitlements getIpEntitlements)
        {
            UserIpAddressContext = userIpAddressContext;
            UserSession = userSession;
            GetIpEntitlements = getIpEntitlements;
        }

        private const string EntitlementSessionKeyPrefix = nameof(AuthenticatedIpContext);

		public string EntitlementSessionKey => $"{EntitlementSessionKeyPrefix}{UserIpAddressContext.IpAddress}";

		public IList<IEntitlement> Entitlements
        {
            get
            {
				var entitlementsSessions = UserSession.Get<IList<IEntitlement>>(EntitlementSessionKey);
				var entitlements = entitlementsSessions.HasValue ? entitlementsSessions.Value : new List<IEntitlement>();

                if (entitlements.Any())
				{
					return entitlements;
				}
                    
                entitlements = GetIpEntitlements.GetEntitlements(UserIpAddressContext.IpAddress);

                if(!entitlements.Any())
				{
					entitlements.Add(new Entitlement { ProductCode = "NONE" });
				}

                Entitlements = entitlements;

                return entitlements;
            }
			set { UserSession.Set(EntitlementSessionKey, value); }
        }

        public void RefreshEntitlements()
        {
            Entitlements = new List<IEntitlement>();
        }
    }
}