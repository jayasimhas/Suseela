using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Jabberwocky.Autofac.Attributes;
using System;

namespace Informa.Library.User.UserPreference
{
    [AutowireService(LifetimeScope = LifetimeScope.PerScope)]
    public class MyViewToggleRedirectUrlFactory : IMyViewToggleRedirectUrlFactory
    {
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUserPreferenceContext UserPreferences;
        public MyViewToggleRedirectUrlFactory(ISiteRootContext siterootContext, IAuthenticatedUserContext authenticatedUserContext,
            IUserPreferenceContext userPreferences)
        {
            SiterootContext = siterootContext;
            AuthenticatedUserContext = authenticatedUserContext;
            UserPreferences = userPreferences;
        }
        public string create()
        {
            if(AuthenticatedUserContext.IsAuthenticated)
            {
                if (UserPreferences.Preferences != null &&
                    UserPreferences.Preferences.PreferredChannels != null && UserPreferences.Preferences.PreferredChannels.Count > 0)
                {
                    //Take user to Personalized home page.
                    return "/personal-home";
                }
                else
                {
                    //Take to MyView settings page
                    return SiterootContext.Item?.MyView_Settings_Page?._Url;
                }
            }

            return string.Empty;
        }
    }
}
