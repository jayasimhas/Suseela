namespace Informa.Library.User.UserPreference
{
    using Authentication;
    using Jabberwocky.Autofac.Attributes;
    using ProductPreferences;
    using SalesforceConfiguration;
    using Site;
    using System;
    using Utilities.CMSHelpers;

    [AutowireService(LifetimeScope.PerScope)]
    public class UserPreferenceContext : IUserPreferenceContext
    {
        private const string sessionKey = "Preferences";

        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IAuthenticatedUserSession UserSession;
        protected readonly IFindUserPreferences FindUserPreferences;
        protected readonly ISetUserPreferences SetUserPreferences;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IAddUserProductPreference AddUserProductPreference;
        protected readonly IGetUserProductPreferences GetUserProductPreferences;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IVerticalRootContext VerticalRootContext;
        public UserPreferenceContext(IAuthenticatedUserContext userContext,
            IAuthenticatedUserSession userSession,
            IFindUserPreferences findUserPreferences,
          ISetUserPreferences setUserPreferences,
          ISalesforceConfigurationContext salesforceConfigurationContext,
          IAddUserProductPreference addUserProductPreference,
          ISiteRootContext siteRootContext,
          IVerticalRootContext verticalRootContext,
          IGetUserProductPreferences getUserProductPreferences)
        {
            UserContext = userContext;
            UserSession = userSession;
            FindUserPreferences = findUserPreferences;
            SetUserPreferences = setUserPreferences;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            AddUserProductPreference = addUserProductPreference;
            SiteRootContext = siteRootContext;
            VerticalRootContext = verticalRootContext;
            GetUserProductPreferences = getUserProductPreferences;
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

                IUserPreferences preferences = SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                    GetUserProductPreferences.GetProductPreferences<UserPreferences>(UserContext.User,
                    VerticalRootContext?.Item?.Vertical_Name,
                    SiteRootContext?.Item?.Publication_Code ?? string.Empty, 
                    ProductPreferenceType.PersonalPreferences) :
                    FindUserPreferences.Find(UserContext.User?.Username ?? string.Empty);

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
            var status = SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                AddUserProductPreference.AddUserContentPreferences(UserContext.User?.Username ?? string.Empty,
                UserContext.User?.AccessToken ?? string.Empty, VerticalRootContext?.Item?.Vertical_Name ?? string.Empty,
                SiteRootContext?.Item?.Publication_Code ?? string.Empty, channelPreferences) :
                SetUserPreferences.Set(UserContext.User?.Username ?? string.Empty, channelPreferences);
            if (status)
                clear();
            return status;

        }
    }
}
