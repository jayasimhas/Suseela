using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.User.Entitlement;
using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;

namespace Informa.Library.User.Authentication.Web
{
	[AutowireService(LifetimeScope.Default)]
	public class WebLoginUser : IWebLoginUser
	{
		protected IAuthenticateUser AuthenticateUser;
		protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;
		protected readonly IWebLoginUserActions LoginActions;

		public WebLoginUser(
			IAuthenticateUser authenticateUser,
			ISitecoreVirtualUsernameFactory virtualUsernameFactory,
			IWebLoginUserActions loginActions)
		{
			AuthenticateUser = authenticateUser;
			VirtualUsernameFactory = virtualUsernameFactory;
			LoginActions = loginActions;
		}

		public IWebLoginUserResult Login(string username, string password, bool persist)
		{
			var result = AuthenticateUser.Authenticate(username, password);
			var state = result.State;
			var authenticatedUser = result.User;
			var success = state == AuthenticateUserResultState.Success;

			if (success)
			{
				var sitecoreUsername = VirtualUsernameFactory.Create(authenticatedUser);
				var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);

				sitecoreVirtualUser.Profile.Email = authenticatedUser.Email;
				sitecoreVirtualUser.Profile.Name = authenticatedUser.Name;
			   
                sitecoreVirtualUser.Profile.Save();

				success = AuthenticationManager.Login(sitecoreVirtualUser.Name, persist);

				if (success)
				{
					LoginActions.Process(authenticatedUser);
				}
			}

			return new WebLoginUserResult
			{
				State = state,
				Success = success,
				User = authenticatedUser
			};
		}                                         
    }
}
