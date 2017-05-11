using Informa.Library.Globalization;
using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Content;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceUpdateUserProductPreference : ISalesforceUpdateUserProductPreference
    {
        private readonly ISalesforceSavedSearchFactory SalesforceSavedSearchRequestFactory;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceInfoLogger InfoLogger;
        protected readonly ISalesforceErrorLogger ErrorLogger;
        private readonly ISalesforceNewsletterFactory SalesforceContentNewsletterFactory;

        public SalesforceUpdateUserProductPreference(
            ISalesforceSavedSearchFactory salesforceSavedSearchRequestFactory,
            ITextTranslator textTranslator,
    ISalesforceConfigurationContext salesforceConfigurationContext,
    ISalesforceInfoLogger infoLogger,
    ISalesforceNewsletterFactory salesforceContentNewsletterFactory,
    ISalesforceErrorLogger errorLogger)
        {
            SalesforceSavedSearchRequestFactory = salesforceSavedSearchRequestFactory;
            TextTranslator = textTranslator;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
            SalesforceContentNewsletterFactory = salesforceContentNewsletterFactory;
            ErrorLogger = errorLogger;
        }
        public IContentResponse UpdateUserSavedSearch(string accessToken, ISavedSearchEntity entity)
        {
            if (entity != null ||
            !new[] { entity.Username, entity.Name, entity.SearchString, entity.UnsubscribeToken, entity.PublicationCode }
            .Any(string.IsNullOrEmpty))
            {
                try
                {
                    Stopwatch swMain = Stopwatch.StartNew();
                    var request = SalesforceSavedSearchRequestFactory.CreateUpdateRequest(entity);
                    if (request != null)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                            var updateUserProductPreferenceEndPoints = SalesforceConfigurationContext?.UpdateUserProductPreferenceEndPoints(entity.Id);
                            InfoLogger.Log(updateUserProductPreferenceEndPoints, this.GetType().Name);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            var requestJson = JsonConvert.SerializeObject(request).ToString();
                            InfoLogger.Log(requestJson, this.GetType().Name);
                            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                            Stopwatch sw = Stopwatch.StartNew();
                            var result = HttpClientExtension.PatchAsync(client, updateUserProductPreferenceEndPoints, content).Result;
                            Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-UpdateUserSavedSearch-Time", sw, "SalesforceAPICall");
                            Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-UpdateUserSavedSearch-Time", swMain, "SalesforceOuterCall");
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
                }
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Update User Saved Search", e);
                    return new ContentResponse
                    {
                        Success = false,
                        Message = "Invalid input has been provided."
                    };
                }
            }

            return new ContentResponse
            {
                Success = false,
                Message = "Invalid input has been provided."
            };
        }

        public bool UpdateNewsletterUserOptIns(string accessToken, string username, string publicationCode, IEnumerable<INewsletterUserOptIn> optIns)
        {
            if (optIns != null ||
            !new[] { username, publicationCode }
            .Any(string.IsNullOrEmpty))
            {
                try
                {
                    Stopwatch swMain = Stopwatch.StartNew();
                    string updateUserProductPreferenceEndPoints = string.Empty;
                    string requestJson = string.Empty;
                    foreach (var item in optIns)
                    {
                        var request = SalesforceContentNewsletterFactory.CreateUpdateRequest(item);
                        if (request != null)
                        {
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                                updateUserProductPreferenceEndPoints = SalesforceConfigurationContext?.UpdateUserProductPreferenceEndPoints(item.SalesforceId);
                                InfoLogger.Log(updateUserProductPreferenceEndPoints, this.GetType().Name);
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                                requestJson = JsonConvert.SerializeObject(request).ToString();
                                InfoLogger.Log(requestJson, this.GetType().Name);
                                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                                Stopwatch sw = Stopwatch.StartNew();
                                var result = HttpClientExtension.PatchAsync(client, updateUserProductPreferenceEndPoints, content).Result;
                                Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-UpdateNewsletterUserOptIns-Time", sw, "SalesforceAPICall");
                                Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-UpdateNewsletterUserOptIns-Time", swMain, "SalesforceOuterCall");
                                InfoLogger.Log(result.ReasonPhrase, this.GetType().Name);

                                if (!result.IsSuccessStatusCode)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call :Update Newsletter User OptIns", e);
                    return false;
                }

            }

            return false;
        }

        public bool UpdateOffersOptIns(string accessToken, string username, string publicationCode, OffersOptIn optIn)
        {
            if (!new[] { username, publicationCode }
            .Any(string.IsNullOrEmpty))
            {
                try
                {
                    Stopwatch swMain = Stopwatch.StartNew();
                    var request = SalesforceContentNewsletterFactory.CreateUpdateRequest(optIn);
                    if (request != null)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                            var updateUserProductPreferenceEndPoints = SalesforceConfigurationContext?.UpdateUserProductPreferenceEndPoints(optIn.SalesforceId);
                            InfoLogger.Log(updateUserProductPreferenceEndPoints, this.GetType().Name);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            var requestJson = JsonConvert.SerializeObject(request).ToString();
                            InfoLogger.Log(requestJson, this.GetType().Name);
                            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
                            Stopwatch sw = Stopwatch.StartNew();
                            var result = HttpClientExtension.PatchAsync(client, updateUserProductPreferenceEndPoints, content).Result;
                            Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-UpdateOffersOptIns-Time", sw, "SalesforceAPICall");
                            Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-UpdateOffersOptIns-Time", swMain, "SalesforceOuterCall");
                            InfoLogger.Log(result.ReasonPhrase, this.GetType().Name);

                            if (!result.IsSuccessStatusCode)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call :Update Offers OptIns", e);
                    return false;
                }
            }

            return false;
        }
    }
}
