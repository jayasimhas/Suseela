using Informa.Library.SalesforceConfiguration;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class SalesforceFindUserInfo : ISalesforceFindUserInfo
    {
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IHttpClientHelper HttpClientHelper;

        public SalesforceFindUserInfo(
            IHttpClientHelper httpClientHelper,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            HttpClientHelper = httpClientHelper;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }

        public ISalesforceUserInfo Find(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return null;
            }

            var userInfoResponse = HttpClientHelper.GetDataResponse<SalesforceUserInfo>(new Uri(SalesforceConfigurationContext?.GetUserInfoEndPoints())
                , new AuthenticationHeaderValue("Authorization", "Bearer " + accessToken),
                new Dictionary<string, string>());

            if (userInfoResponse != null && string.IsNullOrWhiteSpace(userInfoResponse.UserName))
            {
                return null;
            }

            return userInfoResponse;
        }
    }
}
