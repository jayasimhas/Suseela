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
	    protected readonly IGetUserEntitlements GetUserEntitlements;
		protected readonly IWebLoginUserActions LoginActions;

		public WebLoginUser(
			IAuthenticateUser authenticateUser,
			ISitecoreVirtualUsernameFactory virtualUsernameFactory,
            IGetUserEntitlements getUserEntitlements,
			IWebLoginUserActions loginActions)
		{
			AuthenticateUser = authenticateUser;
			VirtualUsernameFactory = virtualUsernameFactory;
		    GetUserEntitlements = getUserEntitlements;
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
				var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, persist);

				sitecoreVirtualUser.Profile.Email = authenticatedUser.Email;
				sitecoreVirtualUser.Profile.Name = authenticatedUser.Name;


			    var entitlements = GetUserEntitlements.GetEntitlements(username, GetIPAddress()) ?? new List<IEntitlement>();                                           
			    sitecoreVirtualUser.Profile.SetCustomProperty(nameof(Entitlement.Entitlement), string.Join(",", entitlements.Select(x => x.ProductCode)));

                sitecoreVirtualUser.Profile.Save();

				success = AuthenticationManager.LoginVirtualUser(sitecoreVirtualUser);

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

        public static String GetIPAddress()
        {
            String ip =
                HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            
            return ip.Split(',').FirstOrDefault();
        }                                          
    }
}
