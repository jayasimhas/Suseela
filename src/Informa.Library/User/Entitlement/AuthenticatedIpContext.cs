using System.Collections.Generic;
using System.Linq;
using Informa.Library.Salesforce.User;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
    [AutowireService(LifetimeScope.Default)]
    public class AuthenticatedIpContext : IAuthenticatedIPContext
    { 
        private readonly IUserIpAddressContext UserIpAddressContext;
        private readonly IUserSession UserSession;
        private readonly IGetIPEntitlements GetIpEntitlements;
        public AuthenticatedIpContext(IUserIpAddressContext userIpAddressContext, IUserSession userSession, IGetIPEntitlements getIpEntitlements)
        {
            UserIpAddressContext = userIpAddressContext;
            UserSession = userSession;
            GetIpEntitlements = getIpEntitlements;
        }

        private const string EntitlementSessionKey = nameof(AuthenticatedIpContext);

        public IList<IEntitlement> Entitlements
        {
            get
            {
                var entitlements = UserSession.Get<IList<IEntitlement>>($"{EntitlementSessionKey}{UserIpAddressContext.IpAddress}") ??
                                   new List<IEntitlement>();

                if (entitlements != null && entitlements.Any())
                    return entitlements;

                entitlements = GetIpEntitlements.GetEntitlements(UserIpAddressContext.IpAddress.ToString());
                if(!entitlements.Any())
                    entitlements.Add(new Entitlement { ProductCode = "NONE" });

                Entitlements = entitlements;
                return entitlements;
            }
            private set { UserSession.Set($"{EntitlementSessionKey}{UserIpAddressContext.IpAddress}", value); }
        }

        public void RefreshEntitlements()
        {
            Entitlements = new List<IEntitlement>();
        }
    }
}