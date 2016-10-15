namespace Informa.Library.User.UserPreference
{
    using Authentication;
    using Jabberwocky.Autofac.Attributes;
    using System;

    [AutowireService(LifetimeScope.PerScope)]
    public class UserPreferenceContext : IUserPreferenceContext
    {
        private const string sessionKey = "Preferences";

        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IAuthenticatedUserSession UserSession;
        protected readonly IFindUserPreferences FindUserPreferences;
        protected readonly ISetUserPreferences SetUserPreferences;
        public UserPreferenceContext(IAuthenticatedUserContext userContext,
            IAuthenticatedUserSession userSession,
            IFindUserPreferences findUserPreferences,
          ISetUserPreferences setUserPreferences)
        {
            UserContext = userContext;
            UserSession = userSession;
            FindUserPreferences = findUserPreferences;
            SetUserPreferences = setUserPreferences;
        }
        public IUserPreferences Preferences
        {
            get
            {
                if (!UserContext.IsAuthenticated)
                {
                    return null;
                }

                var proferencesSession = UserSession.Get<IUserPreferences>(sessionKey);

                if (proferencesSession.HasValue)
                {
                    return proferencesSession.Value;
                }

                var preferences = FindUserPreferences.Find(UserContext.User?.Username ?? string.Empty);

                Preferences = preferences;

                return preferences;
            }

            set
            {
                UserSession.Set(sessionKey, value);
            }
        }

        public void clear()
        {
            UserSession.Clear(sessionKey);
        }

        public bool Set(string channelPreferences)
        {
            if (string.IsNullOrWhiteSpace(channelPreferences))
                return false;
            return SetUserPreferences.Set(UserContext.User?.Username ?? string.Empty, channelPreferences);
        }
    }
}
