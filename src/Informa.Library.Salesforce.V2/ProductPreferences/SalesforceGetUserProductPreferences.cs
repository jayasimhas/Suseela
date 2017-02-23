using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceGetUserProductPreferences : ISalesforceGetUserProductPreferences
    {
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public SalesforceGetUserProductPreferences(ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }
        public T GetProductPreferences<T>(IAuthenticatedUser user, string publication, ProductPreferenceType type)
        {
            if (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.AccessToken)
                && !string.IsNullOrWhiteSpace(publication) && type != ProductPreferenceType.None)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Entitlement_Api_Url?.Url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);
                    var query = string.Empty;
                    HttpResponseMessage response = client.GetAsync(SalesforceConfigurationContext?.GetUserProductPreferencesEndPoints(query)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = response.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrWhiteSpace(responseString))
                        {
                            var productPreferences = JsonConvert.DeserializeObject<ProductPreferencesResult>(responseString);
                            ////return userEntitlements?.Select(entitlement => EntitlementFactoryV2.Create(entitlement) as IEntitlement).ToList();
                        }
                    }
                }
            }
            return default(T);
        }
    }
}
