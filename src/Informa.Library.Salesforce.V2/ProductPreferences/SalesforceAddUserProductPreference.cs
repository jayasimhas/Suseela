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
        protected readonly ISalesforceErrorLogger ErrorLogger;
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
            ISalesforceNewsletterFactory salesforceContentNewsletterFactory,
            ISalesforceErrorLogger errorLogger)
        {
            SalesforceSavedSearchRequestFactory = salesforceSavedSearchRequestFactory;
            SalesforceSaveDocumentFactory = salesforceSaveDocumentFactory;
            TextTranslator = textTranslator;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
            SalesforceContentPreferencesFactory = salesforceContentPreferencesFactory;
            SalesforceDeleteUserProductPreferences = salesforceDeleteUserProductPreferences;
            SalesforceContentNewsletterFactory = salesforceContentNewsletterFactory;
            ErrorLogger = errorLogger;
        }
        public IContentResponse AddUserSavedSearch(string accessToken, ISavedSearchEntity entity)
        {

            if (entity != null ||
              !new[] { entity.Username, entity.Name, entity.SearchString, entity.UnsubscribeToken, entity.PublicationCode }
              .Any(string.IsNullOrEmpty))
            {
                try
                {
                    var request = SalesforceSavedSearchRequestFactory.Create(entity);
                    if (request != null)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                            var addUserProductPreferencesEndPoints = SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints();
                            InfoLogger.Log(addUserProductPreferencesEndPoints, this.GetType().Name);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            var requestJosn = JsonConvert.SerializeObject(request).ToString();
                            InfoLogger.Log(requestJosn, this.GetType().Name);
                            var content = new StringContent(requestJosn, Encoding.UTF8, "application/json");
                            var result = client.PostAsync(addUserProductPreferencesEndPoints, content).Result;

                            if (result.IsSuccessStatusCode)
                            {
                                var responseString = result.Content.ReadAsStringAsync().Result;
                                InfoLogger.Log(responseString, this.GetType().Name);
                                var response = JsonConvert.DeserializeObject<AddProductPreferenceResponse>(responseString);
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
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Add User Saved Search", e);
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

        public bool AddUserContentPreferences(string userName, string accessToken, string verticalPreferenceLocale
            , string publicationCode, string contentPreferences)
        {

            if (!new[] { userName, accessToken, verticalPreferenceLocale, publicationCode, contentPreferences }.Any(string.IsNullOrEmpty))
            {
                try
                {
                    var request = SalesforceContentPreferencesFactory.Create(userName, verticalPreferenceLocale, publicationCode, contentPreferences);
                    if (request != null)
                    {
                        var deleteResponse = SalesforceDeleteUserProductPreferences.DeleteUserProductPreferences(
                            userName, accessToken, publicationCode, ProductPreferenceType.ContentPreferences);
                        if (deleteResponse.Success)
                        {
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                                var addUserProductPreferencesEndPoints = SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints();
                                InfoLogger.Log(addUserProductPreferencesEndPoints, this.GetType().Name);
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                                var requestJosn = JsonConvert.SerializeObject(request).ToString();
                                InfoLogger.Log(requestJosn, this.GetType().Name);
                                var content = new StringContent(requestJosn, Encoding.UTF8, "application/json");
                                var result = client.PostAsync(addUserProductPreferencesEndPoints, content).Result;
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
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Add User Content Preferences", e);
                    return false;
                }
            }

            return false;
        }

        public ISavedDocumentWriteResult AddSavedDocument(string verticalPreferenceLocale, string publicationCode, string Username, string documentName, string documentDescription, string documentId, string accessToken)
        {
            if (!new[] { Username, documentId, documentDescription, documentName }.Any(string.IsNullOrEmpty))
            {
                try
                {
                    var request = SalesforceSaveDocumentFactory.Create(verticalPreferenceLocale, publicationCode, Username, documentName, documentDescription, documentId);

                    if (request != null)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                            var addUserProductPreferencesEndPoints = SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints();
                            InfoLogger.Log(addUserProductPreferencesEndPoints, this.GetType().Name);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            var requestJosn = JsonConvert.SerializeObject(request).ToString();
                            InfoLogger.Log(requestJosn, this.GetType().Name);
                            var content = new StringContent(requestJosn, Encoding.UTF8, "application/json");
                            var result = client.PostAsync(addUserProductPreferencesEndPoints, content).Result;
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
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Add User Saved Document", e);
                    return new SavedDocumentWriteResult
                    {
                        Success = false,
                        Message = "Invalid input has been provided.",
                        SalesforceId = string.Empty
                    };
                }
            }
            return new SavedDocumentWriteResult
            {
                Success = false,
                Message = "Invalid input has been provided.",
                SalesforceId = string.Empty
            };
        }

        public bool AddNewsletterUserOptIns(string verticalPreferenceLocale, string publicationCode, string username, string accessToken, IEnumerable<INewsletterUserOptIn> optIns)
        {
            if ((!new[] { verticalPreferenceLocale, publicationCode, username }.Any(string.IsNullOrEmpty)) && optIns != null)
            {
                try
                {
                    var request = SalesforceContentNewsletterFactory.Create(username, verticalPreferenceLocale, publicationCode, optIns);
                    if (request != null)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                            var addUserProductPreferencesEndPoints = SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints();
                            InfoLogger.Log(addUserProductPreferencesEndPoints, this.GetType().Name);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            var requestJosn = JsonConvert.SerializeObject(request).ToString();
                            InfoLogger.Log(requestJosn, this.GetType().Name);
                            var content = new StringContent(requestJosn, Encoding.UTF8, "application/json");
                            var result = client.PostAsync(addUserProductPreferencesEndPoints, content).Result;
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
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Add User Email Preferences", e);
                    return false;
                }
            }

            return false;
        }


        public bool AddOffersOptIns(string verticalPreferenceLocale, string publicationCode, string username, string accessToken, bool optIn)
        {
            if ((!new[] { verticalPreferenceLocale, publicationCode, username }.Any(string.IsNullOrEmpty)))
            {
                try
                {
                    var request = SalesforceContentNewsletterFactory.Create(username, verticalPreferenceLocale, publicationCode, !optIn);
                    if (request != null)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                            var addUserProductPreferencesEndPoints = SalesforceConfigurationContext?.AddUserProductPreferencesEndPoints();
                            InfoLogger.Log(addUserProductPreferencesEndPoints, this.GetType().Name);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            var requestJosn = JsonConvert.SerializeObject(request).ToString();
                            InfoLogger.Log(requestJosn, this.GetType().Name);
                            var content = new StringContent(requestJosn, Encoding.UTF8, "application/json");
                            var result = client.PostAsync(addUserProductPreferencesEndPoints, content).Result;
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
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Add User Offers Email OptIns", e);
                    return false;
                }
            }

            return false;
        }
    }
}
