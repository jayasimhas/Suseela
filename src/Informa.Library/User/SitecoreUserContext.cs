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
		public Sitecore.Security.Accounts.User User => Context.User;

	    #region Implementation of IEntitlementContext

	    public IList<IEntitlement> Entitlements
	    {
	        get
	        {
                return
                 new List<IEntitlement>(User.Profile.GetCustomProperty(nameof(Entitlement.Entitlement))
                     .Split(',')
                     .Select(x => new Entitlement.Entitlement { ProductCode = x }));
            }
	    }

	    #endregion
	}
}
