using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Informa.Library.User.Authentication;
using System.Web;
using Informa.Library.Salesforce.User.Authentication;
using Informa.Library.SalesforceConfiguration;

namespace Informa.Library.Salesforce.V2.User.Authentication
{
    public class SalesforceAuthenticateUserV2 : ISalesforceAuthenticateUserV2
    {
        protected readonly IHttpClientHelper HttpClientHelper;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        private string tokenUrl = "/services/oauth2/token";
        private string userInforUrlFormat = "{0}/services/oauth2/userinfo";

        public SalesforceAuthenticateUserV2(
            IHttpClientHelper httpClientHelper, ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            HttpClientHelper = httpClientHelper;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public IAuthenticateUserResult Authenticate(string code, string grant_type,
            string client_id, string client_secret, string redirect_uri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Service_Url?.Url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            var pairs = new Dictionary<string, string>
            {
                        { "code", code },
                        { "grant_type", "authorization_code" },
                        { "client_id", client_id },
                        { "client_secret", client_secret },
                        { "redirect_uri", redirect_uri }
                    };
            HttpResponseMessage response = client.PostAsync(tokenUrl,
                                          new FormUrlEncodedContent(pairs)).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            var values = HttpUtility.ParseQueryString(responseString);
            var accessToken = values["access_token"];
            var idToken = values["id_token"];

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return ErrorResult;
            }

            var authenticatedUser = HttpClientHelper.GetDataResponse<UserInfoResult>( new Uri(string.Format(userInforUrlFormat,
                SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Service_Url?.Url))
                , new AuthenticationHeaderValue("Authorization", "Bearer " + accessToken), 
                new Dictionary<string, string>());

            if (authenticatedUser == null)
            {
                return ErrorResult;
            }
            return new SalesforceAuthenticateUserResult
            {
                State = AuthenticateUserResultState.Success,
                User = new SalesforceAuthenticatedUser
                {
                    Username = authenticatedUser.preferred_username,
                    Email = authenticatedUser.email,
                    Name = string.Format("{0} {1}", authenticatedUser.given_name, authenticatedUser.family_name),
                    ////AccountId = userAccount.accounts != null ? userAccount.accounts.Select(x => x.accountId).ToList() : null,
                    ////ContactId = loginResponse.contactId
                }
            };
        }

        public SalesforceAuthenticateUserResult ErrorResult => new SalesforceAuthenticateUserResult
        {
            State = AuthenticateUserResultState.Failure
        };
    }
}
