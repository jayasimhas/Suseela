using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;
using Informa.Library.User.Profile;

namespace Informa.Library.User.Authentication.Web
{
    [AutowireService(LifetimeScope.Default)]
    public class WebLoginUser : IWebLoginUser
    {
        protected readonly IUserProfileFactory UserProfileFactory;
        protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;
        protected readonly IWebLoginUserActions LoginActions;

        public WebLoginUser(
            IUserProfileFactory userProfileFactory,
            ISitecoreVirtualUsernameFactory virtualUsernameFactory,
            IWebLoginUserActions loginActions)
        {
            UserProfileFactory = userProfileFactory;
            VirtualUsernameFactory = virtualUsernameFactory;
            LoginActions = loginActions;
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
                
            }

            sitecoreVirtualUser.Profile.Comment = user.AccessToken;
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
