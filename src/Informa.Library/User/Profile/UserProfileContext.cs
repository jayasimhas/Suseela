using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Profile
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserProfileContext : IUserProfileContext
	{
		private const string sessionKey = "Profile";

		protected readonly IFindUserProfileByUsername FindUserProfile;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;

		public UserProfileContext(
			IFindUserProfileByUsername findUserProfile,
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession)
		{
			FindUserProfile = findUserProfile;
			UserContext = userContext;
			UserSession = userSession;
		}

		public IUserProfile Profile
		{
			get
			{
				var profileSession = UserSession.Get<IUserProfile>(sessionKey);

				if (profileSession.HasValue)
				{
					return profileSession.Value;
				}

				if (!UserContext.IsAuthenticated)
				{
					return null;
				}

				var profile = FindUserProfile.Find(UserContext.User?.Username ?? string.Empty);

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
