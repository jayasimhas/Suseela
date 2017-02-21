using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Profile
{
	[AutowireService(LifetimeScope.PerScope)]
	public class UserProfileContext : IUserProfileContext
	{
		private const string sessionKey = "Profile";

		protected readonly IFindUserProfileByUsername FindUserProfile;
        protected readonly IFindUserProfileByUsernameV2 FindUserProfileV2;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;

		public UserProfileContext(
			IFindUserProfileByUsername findUserProfile,
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
            IFindUserProfileByUsernameV2 findUserProfileV2,
            ISalesforceConfigurationContext salesforceConfigurationContext)
		{
			FindUserProfile = findUserProfile;
			UserContext = userContext;
			UserSession = userSession;
            FindUserProfileV2 = findUserProfileV2;
            SalesforceConfigurationContext = salesforceConfigurationContext;

        }

		public IUserProfile Profile
		{
			get
			{
				if (!UserContext.IsAuthenticated)
				{
					return null;
				}

				var profileSession = UserSession.Get<IUserProfile>(sessionKey);

				if (profileSession.HasValue)
				{
					return profileSession.Value;
				}

				var profile = SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                    FindUserProfileV2.Find(UserContext.User?.AccessToken ?? string.Empty):
                FindUserProfile.Find(UserContext.User?.Username ?? string.Empty);

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
