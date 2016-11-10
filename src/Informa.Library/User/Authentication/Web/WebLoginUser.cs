using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Security.Authentication;
using Informa.Library.User.Profile;
using log4net;
using Newtonsoft.Json;

namespace Informa.Library.User.Authentication.Web
{
	[AutowireService(LifetimeScope.Default)]
	public class WebLoginUser : IWebLoginUser
	{
		protected readonly IUserProfileFactory UserProfileFactory;
		protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;
		protected readonly IWebLoginUserActions LoginActions;
        protected readonly ILog Logger;

        public WebLoginUser(
			IUserProfileFactory userProfileFactory,
			ISitecoreVirtualUsernameFactory virtualUsernameFactory,
			IWebLoginUserActions loginActions,
             ILog logger)
		{
			UserProfileFactory = userProfileFactory;
			VirtualUsernameFactory = virtualUsernameFactory;
			LoginActions = loginActions;
            Logger = logger;
        }

		public IWebLoginUserResult Login(IUser user, bool persist)
		{
            Logger.Error("User data : " + JsonConvert.SerializeObject(user));
            var sitecoreUsername = VirtualUsernameFactory.Create(user);
            Logger.Error("User Sitecore Username : " + sitecoreUsername);
            var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);
            var userProfile = UserProfileFactory.Create(user);
            Logger.Error("User Profile data : " + JsonConvert.SerializeObject(userProfile));
            if (userProfile != null)
			{
				sitecoreVirtualUser.Profile.Email = userProfile.Email;
				sitecoreVirtualUser.Profile.Name = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
			}
			   
            sitecoreVirtualUser.Profile.Save();
            var success = AuthenticationManager.Login(sitecoreVirtualUser.Name, persist);
            Logger.Error("User Login Response : " + success);
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
