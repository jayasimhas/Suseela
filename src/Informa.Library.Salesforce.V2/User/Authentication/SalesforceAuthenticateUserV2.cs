using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Informa.Library.User.Authentication;
using System.Web;
using Informa.Library.Salesforce.User.Authentication;
using Informa.Library.SalesforceConfiguration;
using Informa.Library.Salesforce.V2.User.Entitlement;
using Informa.Library.Salesforce.V2.User.Profile;

namespace Informa.Library.Salesforce.V2.User.Authentication
{
    public class SalesforceAuthenticateUserV2 : ISalesforceAuthenticateUserV2
    {
        protected readonly IHttpClientHelper HttpClientHelper;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceGetUserEntitlementsV2 SalesforceGetUserEntitlementsV2;
        protected readonly ISalesforceFindUserInfo SalesforceFindUserInfo;

        public SalesforceAuthenticateUserV2(
            IHttpClientHelper httpClientHelper,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            ISalesforceGetUserEntitlementsV2 salesforceGetUserEntitlementsV2,
            ISalesforceFindUserInfo salesforceFindUserInfo)
        {
            HttpClientHelper = httpClientHelper;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            SalesforceGetUserEntitlementsV2 = salesforceGetUserEntitlementsV2;
            SalesforceFindUserInfo = salesforceFindUserInfo;
        }

        public IAuthenticateUserResult Authenticate(string code, string grant_type,
            string client_id, string client_secret, string redirect_uri)
        {
            HttpClient client = new HttpClient();
            string accessToken = string.Empty;
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
            HttpResponseMessage response = client.PostAsync(CreateRequestUri(client.BaseAddress.AbsolutePath,
                SalesforceConfigurationContext?.GetUserAccessTokenEndPoints()),
                new FormUrlEncodedContent(pairs)).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(responseString))
                {
                    var values = HttpUtility.ParseQueryString(responseString);
                    accessToken = values["access_token"];
                }
            }
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return ErrorResult;
            }

            var authenticatedUser = SalesforceFindUserInfo.Find(accessToken);

            if (authenticatedUser == null)
            {
                return ErrorResult;
            }

            return new SalesforceAuthenticateUserResult
            {
                State = AuthenticateUserResultState.Success,
                User = new SalesforceAuthenticatedUser
                {
                    Username = authenticatedUser.UserName,
                    Email = authenticatedUser.UserName,
                    Name = authenticatedUser.Name,
                    AccessToken = accessToken
                }
            };
        }

        public SalesforceAuthenticateUserResult ErrorResult => new SalesforceAuthenticateUserResult
        {
            State = AuthenticateUserResultState.Failure
        };

        private string CreateRequestUri(string serviceAbsoluteUri, string urlEndPoint)
        {
            string requestUri = string.Empty;
            requestUri = string.IsNullOrWhiteSpace(serviceAbsoluteUri) ||
                string.IsNullOrWhiteSpace(serviceAbsoluteUri.Replace("/", string.Empty)) ?
                urlEndPoint : string.Format("{0}{1}", serviceAbsoluteUri, urlEndPoint);
            return requestUri;
        }
    }
}
