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
using System.Threading.Tasks;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceUpdateUserProductPreference : ISalesforceUpdateUserProductPreference
    {
        private readonly ISalesforceSavedSearchFactory SalesforceSavedSearchRequestFactory;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceInfoLogger InfoLogger;

        public SalesforceUpdateUserProductPreference(
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
        public IContentResponse UpdateUserSavedSearch(string accessToken, ISavedSearchEntity entity)
        {
            if (entity != null ||
            !new[] { entity.Username, entity.Name, entity.SearchString, entity.UnsubscribeToken, entity.PublicationCode }
            .Any(string.IsNullOrEmpty))
            {
                var request = SalesforceSavedSearchRequestFactory.CreateUpdateRequest(entity);
                if (request != null)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Entitlement_Api_Url?.Url);
                        InfoLogger.Log(SalesforceConfigurationContext?.UpdateUserProductPreferenceEndPoints(entity.Id), this.GetType().Name);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        var content = new StringContent(JsonConvert.SerializeObject(request).ToString(), Encoding.UTF8, "application/json");
                        var result = HttpClientExtension.PatchAsync(client, SalesforceConfigurationContext?.UpdateUserProductPreferenceEndPoints(entity.Id), content).Result;
                        if (result.IsSuccessStatusCode)
                        {
                            InfoLogger.Log(result.ReasonPhrase, this.GetType().Name);
                            return new ContentResponse
                            {
                                Success = true,
                                Message = string.Empty
                            };

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
