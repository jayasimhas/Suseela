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
        protected readonly ISalesforceInfoLogger InfoLogger;
        protected readonly ISalesforceErrorLogger ErrorLogger;

        public SalesforceGetUserEntitlementsV2(
            IHttpClientHelper httpClientHelper,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            ISalesforceEntitlmentFactoryV2 entitlementFactoryV2,
            ISalesforceInfoLogger infologger,
             ISalesforceErrorLogger errorLogger)
        {
            HttpClientHelper = httpClientHelper;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            EntitlementFactoryV2 = entitlementFactoryV2;
            InfoLogger = infologger;
            ErrorLogger = errorLogger;
        }
        public IList<IEntitlement> GetEntitlements(IAuthenticatedUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.AccessToken) && !string.IsNullOrWhiteSpace(user.Username))
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);

                        var getUserEntitlementsEndPoints = SalesforceConfigurationContext?.GetUserEntitlementsEndPoints(user.Username);
                        HttpResponseMessage response = client.GetAsync(getUserEntitlementsEndPoints).Result;
                        InfoLogger.Log(getUserEntitlementsEndPoints, this.GetType().Name);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = response.Content.ReadAsStringAsync().Result;
                            if (!string.IsNullOrWhiteSpace(responseString))
                            {
                                InfoLogger.Log(responseString, this.GetType().Name);
                                var userEntitlements = JsonConvert.DeserializeObject<List<UserEntitlement>>(responseString);
                                return userEntitlements?.Select(entitlement => EntitlementFactoryV2.Create(entitlement) as IEntitlement).ToList();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Get User Entitlements", e);
                    return new List<IEntitlement>();
                }
            }

            return new List<IEntitlement>();
        }
    }
}

