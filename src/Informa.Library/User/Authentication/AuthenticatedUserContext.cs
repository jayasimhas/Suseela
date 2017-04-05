using Informa.Library.SalesforceConfiguration;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Profile;
using Jabberwocky.Glass.Autofac.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Security.Authentication;
using System;
using System.Configuration;
using System.Web;
using System.Web.Security;

namespace Informa.Library.User.Authentication
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class AuthenticatedUserContext : IAuthenticatedUserContext
    {
        protected readonly ISitecoreUserContext SitecoreUserContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IUserSession UserSession;
        protected readonly IUserProfileFactory UserProfileFactory;
        protected readonly IUserProfileFactoryV2 UserProfileFactoryV2;
        protected readonly ISitecoreVirtualUsernameFactory VirtualUsernameFactory;
        protected readonly IVerticalLogin VerticalLogin;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public AuthenticatedUserContext(
            ISitecoreUserContext sitecoreUserContext,
            IGlobalSitecoreService globalService,
            ISiteRootContext siteRootContext,
            IUserSession userSession,
            IUserProfileFactory userProfileFactory,
            ISitecoreVirtualUsernameFactory virtualUsernameFactory,
            IVerticalLogin verticalLogin,
            IUserProfileFactoryV2 userProfileFactoryV2,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            SitecoreUserContext = sitecoreUserContext;
            GlobalService = globalService;
            SiteRootContext = siteRootContext;
            UserSession = userSession;
            UserProfileFactory = userProfileFactory;
            VirtualUsernameFactory = virtualUsernameFactory;
            VerticalLogin = verticalLogin;
            UserProfileFactoryV2 = userProfileFactoryV2;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public IAuthenticatedUser User
        {
            get
            {
                if (!IsAuthenticated) { return null; }

                var sitecoreUser = SitecoreUserContext.User;

                //Although sitecoreUser.Profile.Email gets filled and saved when creating the user, after the browser gets closed and reopened the Email becomes NULL.
                //So had to find another way to retrieve it when extranet
                string email = sitecoreUser.Profile.Email;
                if (string.IsNullOrEmpty(email) && sitecoreUser.IsAuthenticated && sitecoreUser.Domain.Name == "extranet")
                {
                    sitecoreUser.RuntimeSettings.IsVirtual = true;
                    sitecoreUser.Profile.Email = sitecoreUser.LocalName;

                }



                return new AuthenticatedUser
                {
                    Email = sitecoreUser.LocalName,
                    Name = sitecoreUser.Profile.Name,
                    Username = sitecoreUser.LocalName,
                    AccessToken = sitecoreUser.Profile.Comment
                };
            }
        }

        public bool IsAuthenticated => AuthenticatedUserCheck();

        private bool AuthenticatedUserCheck()
        {
            string domain = ConfigurationManager.AppSettings["EnableVerticalLogin"];
            if (domain != "true" || SitecoreUserContext.User.Domain.Name.Equals("sitecore", StringComparison.InvariantCultureIgnoreCase))
            {
                return SitecoreUserContext.User.IsAuthenticated;
            }
            var curVertical = GlobalService.GetVerticalRootAncestor(Sitecore.Context.Item.ID.ToGuid())?._Name;
            VerticalLogin.curVertical = curVertical;
            string cookieName = VerticalLogin.GetVerticalCookieName();
            if (SitecoreUserContext.User.IsAuthenticated && HttpContext.Current.Request.Cookies[cookieName] != null)
                return true;

            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                string username = HttpContext.Current.Request.Cookies[cookieName].Value;
                if (!string.IsNullOrEmpty(username))
                {
                    string uToken = username.Contains("|") ? username.Split('|')[1] : string.Empty;
                    IAuthenticatedUser user = new AuthenticatedUser() { Username = username, AccessToken = uToken };
                    var sitecoreUsername = VirtualUsernameFactory.Create(user);
                    var sitecoreVirtualUser = AuthenticationManager.BuildVirtualUser(sitecoreUsername, true);
                    var userProfile = SalesforceConfigurationContext.IsNewSalesforceEnabled ?
                        UserProfileFactoryV2.Create(user) :
                    UserProfileFactory.Create(user);
                    if (userProfile != null)
                    {
                        sitecoreVirtualUser.Profile.Email = userProfile.Email;
                        sitecoreVirtualUser.Profile.Name = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
                    }

                    sitecoreVirtualUser.Profile.Comment = string.IsNullOrWhiteSpace(user.AccessToken) ? string.Empty : user.AccessToken;
                    sitecoreVirtualUser.Profile.Save();
                    var success = AuthenticationManager.Login(sitecoreVirtualUser.Name, true);
                    return true;
                }
            }
            return false;
        }

    }
}
