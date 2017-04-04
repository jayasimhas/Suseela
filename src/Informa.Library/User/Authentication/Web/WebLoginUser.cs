using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;
using Informa.Library.User.Profile;
using System;
using Informa.Library.SalesforceConfiguration;

namespace Informa.Library.User.Authentication.Web
{
    [AutowireService(LifetimeScope.Default)]
    public class WebLoginUser : IWebLoginUser
    {
        protected readonly IUserProfileFactory UserProfileFactory;
        protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;
        protected readonly ISitecoreUserContext SitecoreUserContext;
        protected readonly IWebLoginUserActions LoginActions;
        protected readonly IUserSession UserSession;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IUserProfileFactoryV2 UserProfileFactoryV2;

        public WebLoginUser(
            IUserProfileFactory userProfileFactory,
            ISitecoreVirtualUsernameFactory virtualUsernameFactory,
            IWebLoginUserActions loginActions, 
            IUserSession userSession, 
            ISitecoreUserContext sitecoreUserContext,
            IUserProfileFactoryV2 userProfileFactoryV2,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            UserProfileFactory = userProfileFactory;
            VirtualUsernameFactory = virtualUsernameFactory;
            LoginActions = loginActions;
            UserSession = userSession;
            SitecoreUserContext = sitecoreUserContext;
            UserProfileFactoryV2 = userProfileFactoryV2;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public IWebLoginUserResult Login(IUser user, bool persist)
        {
            var sitecoreUsername = VirtualUsernameFactory.Create(user);
            var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);
            var userProfile = SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                        UserProfileFactoryV2.Create(user) : 
                        UserProfileFactory.Create(user);
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
           
            string loggedInVertical = string.Empty;
            var sitecoreUsername = VirtualUsernameFactory.Create(user);
            var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);
            
            var userProfile = SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                        UserProfileFactoryV2.Create(user) : UserProfileFactory.Create(user);
            if (userProfile != null)
            {
                sitecoreVirtualUser.Profile.Email = userProfile.Email;
                sitecoreVirtualUser.Profile.Name = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
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
