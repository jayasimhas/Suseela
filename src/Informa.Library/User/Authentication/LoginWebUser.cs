using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.Entitlement;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.Default)]
	public class LoginWebUser : ILoginWebUser
	{
		protected IAuthenticateUser AuthenticateUser;
		protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;
	    protected readonly IUserEntitlements UserEntitlements;

		public LoginWebUser(
			IAuthenticateUser authenticateUser,
			ISitecoreVirtualUsernameFactory virtualUsernameFactory,
            IUserEntitlements userEntitlements)
		{
			AuthenticateUser = authenticateUser;
			VirtualUsernameFactory = virtualUsernameFactory;
		    UserEntitlements = userEntitlements;
		}

		public ILoginWebUserResult Login(string username, string password, bool persist)
		{
			var result = AuthenticateUser.Authenticate(username, password);
			var state = result.State;
			var authenticatedUser = result.User;
			var authenticated = state == AuthenticateUserResultState.Success;

			if (authenticated)
			{
				var sitecoreUsername = VirtualUsernameFactory.Create(authenticatedUser);
				var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, persist);

				sitecoreVirtualUser.Profile.Email = authenticatedUser.Email;
				sitecoreVirtualUser.Profile.Name = authenticatedUser.Name;

			    var entitlements = UserEntitlements.GetEntitlements(username, "") ?? new List<IEntitlement>();                                           
			    sitecoreVirtualUser.Profile.SetCustomProperty(nameof(Entitlement.Entitlement), string.Join(",", entitlements.Select(x => x.ProductCode)));

                sitecoreVirtualUser.Profile.Save();

				AuthenticationManager.LoginVirtualUser(sitecoreVirtualUser);
			}

			return new LoginWebUserResult
			{
				State = state,
				Success = authenticated,
				User = authenticatedUser
			};
		}
	}
}
