using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;

namespace Informa.Library.Salesforce.V2.User.Entitlement
{
    public class SalesforceGetUserEntitlementsV2 : ISalesforceGetUserEntitlementsV2
    {
        protected readonly IHttpClientHelper HttpClientHelper;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceEntitlmentFactoryV2 EntitlementFactoryV2;

        public SalesforceGetUserEntitlementsV2(
            IHttpClientHelper httpClientHelper,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            ISalesforceEntitlmentFactoryV2 entitlementFactoryV2)
        {
            HttpClientHelper = httpClientHelper;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            EntitlementFactoryV2 = entitlementFactoryV2;
        }
        public IList<IEntitlement> GetEntitlements(IAuthenticatedUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.AccessToken) && !string.IsNullOrWhiteSpace(user.Username))
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Entitlement_Api_Url?.Url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);
                HttpResponseMessage response = client.GetAsync(SalesforceConfigurationContext?.GetUserEntitlementsEndPoints(user.Username)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        var userEntitlements = JsonConvert.DeserializeObject<List<UserEntitlement>>(responseString);
                        return userEntitlements?.Select(entitlement => EntitlementFactoryV2.Create(entitlement) as IEntitlement).ToList();
                    }
                }
            }

            return new List<IEntitlement>();
        }
    }
}

