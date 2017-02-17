using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;
using Informa.Library.User.Profile;
using System;

namespace Informa.Library.User.Authentication.Web
{
    [AutowireService(LifetimeScope.Default)]
    public class WebLoginUser : IWebLoginUser
    {
        protected readonly IUserProfileFactory UserProfileFactory;
        protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;
        protected readonly IWebLoginUserActions LoginActions;
        protected readonly IUserSession UserSession;

        public WebLoginUser(
            IUserProfileFactory userProfileFactory,
            ISitecoreVirtualUsernameFactory virtualUsernameFactory,
            IWebLoginUserActions loginActions, IUserSession userSession)
        {
            UserProfileFactory = userProfileFactory;
            VirtualUsernameFactory = virtualUsernameFactory;
            LoginActions = loginActions;
            UserSession = userSession;
        }

        public IWebLoginUserResult Login(IUser user, bool persist)
        {
            var sitecoreUsername = VirtualUsernameFactory.Create(user);
            var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);
            var userProfile = UserProfileFactory.Create(user);
            if (userProfile != null)
            {
                sitecoreVirtualUser.Profile.Email = userProfile.Email;
                sitecoreVirtualUser.Profile.Name = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
                //sitecoreVirtualUser.Profile.SetCustomProperty("vertical", "agri");
            }

            sitecoreVirtualUser.Profile.Comment = string.IsNullOrWhiteSpace(user.AccessToken) ? string.Empty : user.AccessToken;
            sitecoreVirtualUser.Profile.Save();
            var success = AuthenticationManager.Login(sitecoreVirtualUser.Name, persist);
            if (success)
            {
                LoginActions.Process(user);
            }

            return new WebLoginUserResult
            {
                Success = success
            };
        }
        public IWebLoginUserResult Login(IUser user, bool persist, string verticalName)
        {
            var sitecoreUsername = VirtualUsernameFactory.Create(user);
            var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);
            
            var userProfile = UserProfileFactory.Create(user);
            if (userProfile != null)
            {
                sitecoreVirtualUser.Profile.Email = userProfile.Email;
                sitecoreVirtualUser.Profile.Name = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
                //Setting the vertical name as part of the user profile object
                sitecoreVirtualUser.Profile.SetCustomProperty("vertical", verticalName);
                // Setting the in user session for double check in the authenticated user context.
                UserSession.Set("user_vertical", verticalName);
            }
            sitecoreVirtualUser.Profile.Comment = string.IsNullOrWhiteSpace(user.AccessToken) ? string.Empty : user.AccessToken;
            sitecoreVirtualUser.Profile.Save();
            var success = AuthenticationManager.Login(sitecoreVirtualUser.Name, persist);
            if (success)
            {
                LoginActions.Process(user);
            }

            return new WebLoginUserResult
            {
                Success = success
            };
        }
    }
}
