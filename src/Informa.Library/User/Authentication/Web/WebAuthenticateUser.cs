﻿using Informa.Library.SalesforceConfiguration;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Configuration;
using System.Web;

namespace Informa.Library.User.Authentication.Web
{
    [AutowireService(LifetimeScope.Default)]
    public class WebAuthenticateUser : IWebAuthenticateUser
    {
        protected readonly IAuthenticateUser AuthenticateUser;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IAuthenticateUserV2 AuthenticateUserV2;
        protected readonly IWebLoginUser LoginWebUser;
        protected readonly IUserSession UserSession;
        private const string sessionKey = nameof(WebAuthenticateUser);
        public WebAuthenticateUser(
            IAuthenticateUser authenticateUser,
            IWebLoginUser loginWebUser,
            IUserSession userSession,
            IAuthenticateUserV2 authenticateUserV2,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            AuthenticateUser = authenticateUser;
            LoginWebUser = loginWebUser;
            UserSession = userSession;
            AuthenticateUserV2 = authenticateUserV2;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public IAuthenticatedUser AuthenticatedUser
        {
            get
            {
                var authenticatedUserSession = UserSession.Get<IAuthenticatedUser>(sessionKey);

                if (authenticatedUserSession.HasValue)
                {
                    return authenticatedUserSession.Value;
                }
                return null;

            }
            set { UserSession.Set(sessionKey, value); }
        }


        public IWebAuthenticateUserResult Authenticate(string username, string password, bool persist)
        {
            var authenticateResult = AuthenticateUser.Authenticate(username, password);
            var state = authenticateResult.State;

            var authenticatedUser = authenticateResult.User;
            var success = state == AuthenticateUserResultState.Success;

            if (success)
            {
                var loginResult = LoginWebUser.Login(authenticatedUser, persist);
                success = loginResult.Success;
                AuthenticatedUser = authenticatedUser;
            }

            return new WebAuthenticateUserResult
            {
                State = state,
                Success = success,
                User = authenticatedUser
            };
        }

        public IWebAuthenticateUserResult Authenticate(string username, string password, bool persist, string vertical)
        {
            var authenticateResult = AuthenticateUser.Authenticate(username, password);
            var state = authenticateResult.State;

            var authenticatedUser = authenticateResult.User;
            var success = state == AuthenticateUserResultState.Success;

            if (success)
            {
                var loginResult = LoginWebUser.Login(authenticatedUser, persist, vertical);
                success = loginResult.Success;
                AuthenticatedUser = authenticatedUser;
            }

            return new WebAuthenticateUserResult
            {
                State = state,
                Success = success,
                User = authenticatedUser
            };
        }

        public IWebAuthenticateUserResult Authenticate(string code, string redirectUrl, string vertical)
        {
            var authenticateResult = AuthenticateUserV2.Authenticate(code, "authorization_code",
                SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Username,
                SalesforceConfigurationContext?.SalesForceConfiguration?.Salesforce_Session_Factory_Password,
                redirectUrl);
            var state = authenticateResult.State;

            var authenticatedUser = authenticateResult.User;
            var success = state == AuthenticateUserResultState.Success;

            if (success)
            {
                var loginResult = LoginWebUser.Login(authenticatedUser, false);
                success = loginResult.Success;
                AuthenticatedUser = authenticatedUser;
            }
            //Current vertical cookiename
            string cookieName = vertical + "_LoggedInUser";
            //Current Vertical subdomain
            string domain = ConfigurationManager.AppSettings[vertical];

            HttpCookie LoggedinKeyCookie = new HttpCookie(vertical + "_LoggedInUser");
            LoggedinKeyCookie.Value = authenticatedUser.Username+"|"+ authenticatedUser.AccessToken;
            LoggedinKeyCookie.Expires = System.DateTime.Now.AddDays(1);
            LoggedinKeyCookie.Domain = domain;
            HttpContext.Current.Response.Cookies.Add(LoggedinKeyCookie);

            return new WebAuthenticateUserResult
            {
                State = state,
                Success = success,
                User = authenticatedUser
            };
        }
        public IWebAuthenticateUserResult Authenticate(IAuthenticatedUser user)
        {
            var loginResult = LoginWebUser.Login(user, false);
            bool success = loginResult.Success;
            return new WebAuthenticateUserResult
            {
                State = AuthenticateUserResultState.Success,
                Success = success,
                User = user
            };
        }
    }
}
