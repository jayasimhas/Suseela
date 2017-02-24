using Informa.Library.Globalization;
using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Content;
using Informa.Library.User.ProductPreferences;
using Informa.Library.User.Search;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceAddUserProductPreference : ISalesforceAddUserProductPreference
    {
        private readonly ISalesforceSavedSearchFactory SalesforceSavedSearchRequestFactory;
        private readonly ISalesforceContentPreferencesFactory SalesforceContentPreferencesFactory;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceInfoLogger InfoLogger;
        protected readonly ISalesforceDeleteUserProductPreferences SalesforceDeleteUserProductPreferences;

        public SalesforceAddUserProductPreference(
            ISalesforceSavedSearchFactory salesforceSavedSearchRequestFactory,
            ITextTranslator textTranslator,
    ISalesforceConfigurationContext salesforceConfigurationContext,
    ISalesforceInfoLogger infoLogger,
    ISalesforceContentPreferencesFactory salesforceContentPreferencesFactory,
    ISalesforceDeleteUserProductPreferences salesforceDeleteUserProductPreferences)
        {
            SalesforceSavedSearchRequestFactory = salesforceSavedSearchRequestFactory;
            TextTranslator = textTranslator;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
            SalesforceContentPreferencesFactory = salesforceContentPreferencesFactory;
            SalesforceDeleteUserProductPreferences = salesforceDeleteUserProductPreferences;
        }
        public IContentResponse AddUserSavedSearch(string accessToken, ISavedSearchEntity entity)
        {

            if (entity != null ||
              !new[] { entity.Username, entity.Name, entity.SearchString, entity.UnsubscribeToken, entity.PublicationCode }
              .Any(string.IsNullOrEmpty))
            {
                var request = SalesforceSavedSearchRequestFactory.Create(entity);
                if (request != null)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Entitlement_Api_Url?.Url);
                        InfoLogger.Log(SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints(), this.GetType().Name);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        var content = new StringContent(JsonConvert.SerializeObject(request).ToString(), Encoding.UTF8, "application/json");
                        var result = client.PostAsync(SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints(), content).Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var responseString = result.Content.ReadAsStringAsync().Result;
                            var response = JsonConvert.DeserializeObject<AddProductPreferenceResponse>(responseString);
                            InfoLogger.Log(responseString, this.GetType().Name);
                            if (!response.hasErrors)
                            {
                                return new ContentResponse
                                {
                                    Success = true,
                                    Message = string.Empty
                                };
                            }
                        }
                    }
                }
            }

            return new ContentResponse
            {
                Success = false,
                Message = "Invalid input has been provided."
            };
        }

        public bool AddUserContentPreferences(string userName, string accessToken, string verticleCode
            , string publicationCode, string contentPreferences)
        {

            if (!new[] { userName, accessToken, verticleCode, publicationCode, contentPreferences }.Any(string.IsNullOrEmpty))
            {
                var request = SalesforceContentPreferencesFactory.Create(userName, verticleCode, publicationCode, contentPreferences);
                if (request != null)
                {
                    var deleteResponse = SalesforceDeleteUserProductPreferences.DeleteUserProductPreferences(
                        userName, accessToken, publicationCode, ProductPreferenceType.PersonalPreferences);
                    if (deleteResponse.Success)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Entitlement_Api_Url?.Url);
                            InfoLogger.Log(SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints(), this.GetType().Name);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            var content = new StringContent(JsonConvert.SerializeObject(request).ToString(), Encoding.UTF8, "application/json");
                            var result = client.PostAsync(SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints(), content).Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var responseString = result.Content.ReadAsStringAsync().Result;
                                var response = JsonConvert.DeserializeObject<AddProductPreferenceResponse>(responseString);
                                InfoLogger.Log(responseString, this.GetType().Name);
                                if (!response.hasErrors)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
