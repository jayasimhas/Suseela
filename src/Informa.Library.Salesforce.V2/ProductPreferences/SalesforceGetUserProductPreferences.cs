﻿using Informa.Library.SalesforceConfiguration;
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
        protected readonly ISalesforceGetUserProductPreferencesQueryFactory SalesforceGetUserProductPreferencesQueryFactory;
        private readonly ISalesforceSavedSearchFactory SalesforceSavedSearchRequestFactory;
        private readonly ISalesforceContentPreferencesFactory SalesforceContentPreferencesFactory;
        private readonly ISalesforceSaveDocumentFactory SalesforceSaveDocumentFactory;
        private readonly ISalesforceNewsletterFactory SalesforceContentNewsletterFactory;

        public SalesforceGetUserProductPreferences(ISalesforceConfigurationContext salesforceConfigurationContext,
            ISalesforceGetUserProductPreferencesQueryFactory salesforceGetUserProductPreferencesQueryFactory,
            ISalesforceSavedSearchFactory salesforceSavedSearchRequestFactory,
            ISalesforceContentPreferencesFactory salesforceContentPreferencesFactory,
            ISalesforceSaveDocumentFactory salesforceSaveDocumentFactory,
            ISalesforceNewsletterFactory salesforceContentNewsletterFactory)
        {
            SalesforceConfigurationContext = salesforceConfigurationContext;
            SalesforceGetUserProductPreferencesQueryFactory = salesforceGetUserProductPreferencesQueryFactory;
            SalesforceSavedSearchRequestFactory = salesforceSavedSearchRequestFactory;
            SalesforceContentPreferencesFactory = salesforceContentPreferencesFactory;
            SalesforceSaveDocumentFactory = salesforceSaveDocumentFactory;
            SalesforceContentNewsletterFactory = salesforceContentNewsletterFactory;
        }
        public T GetProductPreferences<T>(IAuthenticatedUser user, string verticalPreferenceLocale, string publicationCode, ProductPreferenceType type)
        {
            if (!string.IsNullOrWhiteSpace(user.Username) && !string.IsNullOrWhiteSpace(user.AccessToken)
                && !string.IsNullOrWhiteSpace(publicationCode) && !string.IsNullOrWhiteSpace(verticalPreferenceLocale) &&
                type != ProductPreferenceType.None)
            {
                var query = SalesforceGetUserProductPreferencesQueryFactory.Create(
                      user.Username, verticalPreferenceLocale, publicationCode, type);
                if (!string.IsNullOrWhiteSpace(query))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.AccessToken);

                        HttpResponseMessage response = client.GetAsync(SalesforceConfigurationContext?.GetUserProductPreferencesEndPoints(query)).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = response.Content.ReadAsStringAsync().Result;
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
                                if(type == ProductPreferenceType.SavedDocuments)
                                {
                                    return (T)SalesforceSaveDocumentFactory.Create(productPreferences);
                                }
                                if(type == ProductPreferenceType.EmailPreference)
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
            return default(T);
        }
    }
}
