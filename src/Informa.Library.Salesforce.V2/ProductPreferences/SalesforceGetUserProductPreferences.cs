using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Informa.Library.User.ProductPreferences;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceGetUserProductPreferences : ISalesforceGetUserProductPreferences
    {
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceGetUserProductPreferencesQueryFactory SalesforceGetUserProductPreferencesQueryFactory;
        private readonly ISalesforceSavedSearchFactory SalesforceSavedSearchRequestFactory;
        private readonly ISalesforceContentPreferencesFactory SalesforceContentPreferencesFactory;
        private readonly ISalesforceSaveDocumentFactory SalesforceSaveDocumentFactory;
        private readonly ISalesforceNewsletterFactory SalesforceContentNewsletterFactory;
        protected readonly ISalesforceErrorLogger ErrorLogger;
        protected readonly ISalesforceInfoLogger InfoLogger;

        public SalesforceGetUserProductPreferences(ISalesforceConfigurationContext salesforceConfigurationContext,
            ISalesforceGetUserProductPreferencesQueryFactory salesforceGetUserProductPreferencesQueryFactory,
            ISalesforceSavedSearchFactory salesforceSavedSearchRequestFactory,
            ISalesforceContentPreferencesFactory salesforceContentPreferencesFactory,
            ISalesforceSaveDocumentFactory salesforceSaveDocumentFactory,
            ISalesforceNewsletterFactory salesforceContentNewsletterFactory,
            ISalesforceErrorLogger errorLogger,
            ISalesforceInfoLogger infoLogger)
        {
            SalesforceConfigurationContext = salesforceConfigurationContext;
            SalesforceGetUserProductPreferencesQueryFactory = salesforceGetUserProductPreferencesQueryFactory;
            SalesforceSavedSearchRequestFactory = salesforceSavedSearchRequestFactory;
            SalesforceContentPreferencesFactory = salesforceContentPreferencesFactory;
            SalesforceSaveDocumentFactory = salesforceSaveDocumentFactory;
            SalesforceContentNewsletterFactory = salesforceContentNewsletterFactory;
            ErrorLogger = errorLogger;
            InfoLogger = infoLogger;
        }
        public T GetProductPreferences<T>(IAuthenticatedUser user, string verticalPreferenceLocale, string publicationCode, ProductPreferenceType type)
        {
            if (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.AccessToken)
                && !string.IsNullOrWhiteSpace(publicationCode) && !string.IsNullOrWhiteSpace(verticalPreferenceLocale) &&
                type != ProductPreferenceType.None)
            {
                try
                {
                    Stopwatch swMain = Stopwatch.StartNew();
                    var query = SalesforceGetUserProductPreferencesQueryFactory.Create(
                          user.Username, verticalPreferenceLocale, publicationCode, type);
                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);

                            var getUserProductPreferencesEndPoints = SalesforceConfigurationContext?.GetUserProductPreferencesEndPoints(query);
                            InfoLogger.Log(getUserProductPreferencesEndPoints, this.GetType().Name);
                            Stopwatch sw = Stopwatch.StartNew();
                            HttpResponseMessage response = client.GetAsync(getUserProductPreferencesEndPoints).Result;
                            Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-GetProductPreferences-Time", sw, "SalesforceAPICall");
                            Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-GetProductPreferences-Time", swMain, "SalesforceOuterCall");
                            if (response.IsSuccessStatusCode)
                            {
                                var responseString = response.Content.ReadAsStringAsync().Result;
                                InfoLogger.Log(responseString, this.GetType().Name);
                                if (!string.IsNullOrWhiteSpace(responseString))
                                {
                                    var productPreferences = JsonConvert.DeserializeObject<ProductPreferencesResult>(responseString);
                                    if (type == ProductPreferenceType.SavedSearches)
                                    {
                                        return (T)SalesforceSavedSearchRequestFactory.Create(productPreferences);
                                    }
                                    else if (type == ProductPreferenceType.ContentPreferences)
                                    {
                                        return (T)SalesforceContentPreferencesFactory.Create(productPreferences);
                                    }
                                    if (type == ProductPreferenceType.SavedDocuments)
                                    {
                                        return (T)SalesforceSaveDocumentFactory.Create(productPreferences);
                                    }
                                    if (type == ProductPreferenceType.EmailPreference)
                                    {
                                        return (T)SalesforceContentNewsletterFactory.Create(productPreferences);
                                    }
                                    if (type == ProductPreferenceType.EmailSignUp)
                                    {
                                        return (T)SalesforceContentNewsletterFactory.CreateOfferOptinGetRequest(productPreferences);
                                    }
                                }

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Get Product Preferences", e);
                    return default(T);
                }
            }
            return default(T);
        }
    }
}
