using Informa.Library.Globalization;
using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Content;
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
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceInfoLogger InfoLogger;

        public SalesforceAddUserProductPreference(
            ISalesforceSavedSearchFactory salesforceSavedSearchRequestFactory,
            ITextTranslator textTranslator,
    ISalesforceConfigurationContext salesforceConfigurationContext,
    ISalesforceInfoLogger infoLogger)
        {
            SalesforceSavedSearchRequestFactory = salesforceSavedSearchRequestFactory;
            TextTranslator = textTranslator;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
        }
        public IContentResponse AddUserSavedSearch(string accessToken, ISavedSearchEntity entity)
        {

            if (entity != null ||
              !new[] { entity.Username, entity.Name, entity.SearchString, entity.UnsubscribeToken }
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
    }
}
