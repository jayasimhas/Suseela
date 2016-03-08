using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
	    protected readonly IGetUserEntitlements GetUserEntitlements;

		public LoginWebUser(
			IAuthenticateUser authenticateUser,
			ISitecoreVirtualUsernameFactory virtualUsernameFactory,
            IGetUserEntitlements getUserEntitlements)
		{
			AuthenticateUser = authenticateUser;
			VirtualUsernameFactory = virtualUsernameFactory;
		    GetUserEntitlements = getUserEntitlements;
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


			    var entitlements = GetUserEntitlements.GetEntitlements(username, GetIPAddress()) ?? new List<IEntitlement>();                                           
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
