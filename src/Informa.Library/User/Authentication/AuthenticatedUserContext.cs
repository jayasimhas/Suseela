using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.Entitlement;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class AuthenticatedUserContext : IAuthenticatedUserContext
	{
		protected readonly ISitecoreUserContext SitecoreUserContext;

		public AuthenticatedUserContext(
			ISitecoreUserContext sitecoreUserContext)
		{
			SitecoreUserContext = sitecoreUserContext;
		}

		public IAuthenticatedUser User
		{
			get
			{
				var sitecoreUser = SitecoreUserContext.User;

				return new AuthenticatedUser
				{
					Email = sitecoreUser.Profile.Email,
					Name = sitecoreUser.Profile.Name,
					Username = sitecoreUser.Profile.UserName       
				};
			}
		}

		public bool IsAuthenticated => SitecoreUserContext.User.IsAuthenticated;

	    public IList<IEntitlement> Entitlements
	        =>
	            new List<IEntitlement>(SitecoreUserContext.User.Profile.GetCustomProperty(nameof(Entitlement.Entitlement))
	                .Split(',')
	                .Select(x => new Entitlement.Entitlement {ProductCode = x}));
	}

    //public class EntitledUserContext : IEntitledUserContext
    //{
    //    protected readonly IAuthenticatedUserContext
    //    public EntitledUserContext(IAuthenticatedUserContext authenticatedUserContext)
    //    {
            
    //    }

    //    #region Implementation of IAuthenticatedUserContext

    //    public IAuthenticatedUser User { get; }
    //    public bool IsAuthenticated { get; }

    //    #endregion

    //    #region Implementation of IEntitledUserContext

    //    public IEntitlement Entitlement { get; }

    //    #endregion
    //}

    //public interface IEntitledUserContext : IAuthenticatedUserContext
    //{
    //    IEntitlement Entitlement { get; }
    //}
}
