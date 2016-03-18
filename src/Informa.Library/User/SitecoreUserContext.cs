using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.Entitlement;
using Sitecore;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class SitecoreUserContext : ISitecoreUserContext
    {
        private readonly IUserSession UserSession;
        private readonly IGetUserEntitlements GetUserEntitlements;
        private readonly IUserIpAddressContext UserIpAddressContext;

        public SitecoreUserContext(IUserSession userSession, IGetUserEntitlements getUserEntitlements,
            IUserIpAddressContext userIpAddressContext)
        {
            UserSession = userSession;
            GetUserEntitlements = getUserEntitlements;
            UserIpAddressContext = userIpAddressContext;
        }

        private const string EntitlementSessionKey = nameof(SitecoreUserContext);
        public Sitecore.Security.Accounts.User User => Context.User;

        #region Implementation of IEntitlementContext

        public void RefreshEntitlements()
        {
            Entitlements = new List<IEntitlement>();
        }

        private IList<IEntitlement> entitlements = new List<IEntitlement>();
        public IList<IEntitlement> Entitlements
        {
            get
            {
                if (!entitlements.Any())
                    Entitlements = GetEntitlements().FirstOrDefault(x => x.Any());
                return entitlements;
            }
            private set
            {
                entitlements = value;
                UserSession.Set($"{EntitlementSessionKey}", entitlements);
            }
        }

        private IEnumerable<IList<IEntitlement>> GetEntitlements()
        {
            //Get from current instance
            yield return entitlements;
            if (User.IsAuthenticated)
            {
                //Get from Session
                yield return UserSession.Get<IList<IEntitlement>>(EntitlementSessionKey);
                //Get from Salesforce
                yield return
                    GetUserEntitlements.GetEntitlements(User.LocalName, UserIpAddressContext.IpAddress.ToString());
            }
            //Not entitled
            yield return new List<IEntitlement> { new Entitlement.Entitlement { ProductCode = "NONE" } };
        }                      
    }

    #endregion
}
