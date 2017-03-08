using Informa.Library.Globalization;
using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Content;
using Informa.Library.User.Document;
using Informa.Library.User.Newsletter;
using Informa.Library.User.ProductPreferences;
using Informa.Library.User.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceAddUserProductPreference : ISalesforceAddUserProductPreference
    {
        protected readonly ISalesforceSaveDocumentFactory SalesforceSaveDocumentFactory;
        protected readonly ISalesforceSavedSearchFactory SalesforceSavedSearchRequestFactory;
        private readonly ISalesforceContentPreferencesFactory SalesforceContentPreferencesFactory;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceInfoLogger InfoLogger;
        protected readonly ISalesforceDeleteUserProductPreferences SalesforceDeleteUserProductPreferences;
        protected readonly ISalesforceNewsletterFactory SalesforceContentNewsletterFactory;
        public SalesforceAddUserProductPreference(
            ISalesforceSaveDocumentFactory salesforceSaveDocumentFactory,
            ITextTranslator textTranslator,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            ISalesforceInfoLogger infoLogger,
            ISalesforceContentPreferencesFactory salesforceContentPreferencesFactory,
            ISalesforceDeleteUserProductPreferences salesforceDeleteUserProductPreferences,
            ISalesforceSavedSearchFactory salesforceSavedSearchRequestFactory,
            ISalesforceNewsletterFactory salesforceContentNewsletterFactory)
        {
            SalesforceSavedSearchRequestFactory = salesforceSavedSearchRequestFactory;
            SalesforceSaveDocumentFactory = salesforceSaveDocumentFactory;
            TextTranslator = textTranslator;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
            SalesforceContentPreferencesFactory = salesforceContentPreferencesFactory;
            SalesforceDeleteUserProductPreferences = salesforceDeleteUserProductPreferences;
            SalesforceContentNewsletterFactory = salesforceContentNewsletterFactory;
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

        public ISavedDocumentWriteResult AddSavedDocument(string verticalName, string publicationCode, string Username, string documentName, string documentDescription, string documentId, string accessToken)
        {
            if (!new[] { Username, documentId, documentDescription, documentName }.Any(string.IsNullOrEmpty))
            {
                var request = SalesforceSaveDocumentFactory.Create(verticalName, publicationCode, Username, documentName, documentDescription, documentId);

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
                                return new SavedDocumentWriteResult
                                {
                                    Success = true,
                                    Message = string.Empty,
                                    SalesforceId = response.results != null ?
                                    !string.IsNullOrEmpty(response.results.FirstOrDefault().id) ?
                                    response.results.FirstOrDefault().id : string.Empty : string.Empty
                                };
                            }
                        }
                    }
                }
            }
            return new SavedDocumentWriteResult
            {
                Success = false,
                Message = "Invalid input has been provided.",
                SalesforceId = string.Empty
            };
        }

        public bool AddNewsletterUserOptIns(string verticalName, string publicationCode, string username, string accessToken,IEnumerable<INewsletterUserOptIn> optIns)
        {
            if ((!new[] { verticalName, publicationCode, username }.Any(string.IsNullOrEmpty)) && optIns != null)
            {
                var request = SalesforceContentNewsletterFactory.Create(username, verticalName, publicationCode, optIns);
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
                                return true;
                            }
                        }

                    }
                }
            }

            return false;
        }


        public bool AddOffersOptIns(string verticalName, string publicationCode, string username, string accessToken, bool optIn)
        {
            if ((!new[] { verticalName, publicationCode, username }.Any(string.IsNullOrEmpty)))
            {
                var request = SalesforceContentNewsletterFactory.Create(username, verticalName, publicationCode, !optIn);
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
                                return true;
                            }
                        }

                    }
                }
            }

            return false;
        }
    }
}
