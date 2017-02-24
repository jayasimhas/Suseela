using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.User.ProductPreferences;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceDeleteUserProductPreferences : ISalesforceDeleteUserProductPreferences
    {
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceDeleteUserProductPreferencesQueryFactory SalesforceDeleteUserProductPreferencesQueryFactory;
        protected readonly ISalesforceInfoLogger InfoLogger;

        public SalesforceDeleteUserProductPreferences(
    ISalesforceConfigurationContext salesforceConfigurationContext,
    ISalesforceInfoLogger infoLogger,
    ISalesforceDeleteUserProductPreferencesQueryFactory salesforceDeleteUserProductPreferencesQueryFactory)
        {
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
            SalesforceDeleteUserProductPreferencesQueryFactory = salesforceDeleteUserProductPreferencesQueryFactory;
        }
        public IContentResponse DeleteUserProductPreference(string accessToken, string itemId)
        {
            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(itemId))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Entitlement_Api_Url?.Url);
                    InfoLogger.Log(SalesforceConfigurationContext?.DeleteUserProductPreferenceEndPoints(itemId), this.GetType().Name);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var result = client.DeleteAsync(SalesforceConfigurationContext?.DeleteUserProductPreferenceEndPoints(itemId)).Result;
                    InfoLogger.Log(result.ReasonPhrase, this.GetType().Name);
                    if (result.IsSuccessStatusCode)
                    {
                        return new ContentResponse
                        {
                            Success = true,
                            Message = string.Empty
                        };
                    }
                }
            }
            return new ContentResponse
            {
                Success = false,
                Message = "Invalid input has been provided."
            };
        }

        public IContentResponse DeleteUserProductPreferences(string userName, string accessToken, string publicationCode, ProductPreferenceType type)
        {

            return new ContentResponse
            {
                Success = false,
                Message = "Invalid input has been provided."
            };
        }
    }
}
