using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Profile
{
	[AutowireService(LifetimeScope.PerScope)]
	public class UserProfileContext : IUserProfileContext
	{
		private const string sessionKey = "Profile";

		protected readonly IFindUserProfileByUsername FindUserProfile;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;
        protected IWebAuthenticateUser WebAuthenticateUser;

        public UserProfileContext(
			IFindUserProfileByUsername findUserProfile,
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
            IWebAuthenticateUser webAuthenticateUser)
		{
			FindUserProfile = findUserProfile;
			UserContext = userContext;
			UserSession = userSession;
            WebAuthenticateUser = webAuthenticateUser;
        }

		public IUserProfile Profile
		{
			get
			{
				if (!UserContext.IsAuthenticated)
				{
					return null;
				}

				///var profileSession = UserSession.Get<IUserProfile>(sessionKey);

				//if (profileSession.HasValue)
				//{
				//	return profileSession.Value;
				//}

               ////	var profile = FindUserProfile.Find(UserContext.User?.Username ?? string.Empty);

                var sfUser = WebAuthenticateUser.AuthenticatedUser;

                string userId = sfUser?.UserId;
                string url = sfUser?.SalesForceURL;
                string sessionId = sfUser?.SalesForceSessionId;

                var profile = FindUserProfile.Find(userId,url,sessionId);

                Profile = profile;

				return profile;
			}
			set
			{
				UserSession.Set(sessionKey, value);
			}
		}

	    public void Clear()
	    {
	        UserSession.Clear(sessionKey);
	    }
	}
}
