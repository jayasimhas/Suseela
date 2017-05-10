using Informa.Library.SalesforceConfiguration;
using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.User.Document;
using Informa.Library.User.ProductPreferences;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public class SalesforceDeleteUserProductPreferences : ISalesforceDeleteUserProductPreferences
    {
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly ISalesforceDeleteUserProductPreferencesQueryFactory SalesforceDeleteUserProductPreferencesQueryFactory;
        protected readonly ISalesforceInfoLogger InfoLogger;
        protected readonly ISalesforceErrorLogger ErrorLogger;

        public SalesforceDeleteUserProductPreferences(
    ISalesforceConfigurationContext salesforceConfigurationContext,
    ISalesforceInfoLogger infoLogger,
    ISalesforceDeleteUserProductPreferencesQueryFactory salesforceDeleteUserProductPreferencesQueryFactory,
    ISalesforceErrorLogger errorLogger)
        {
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
            SalesforceDeleteUserProductPreferencesQueryFactory = salesforceDeleteUserProductPreferencesQueryFactory;
            ErrorLogger = errorLogger;
        }
        public IContentResponse DeleteUserProductPreference(string accessToken, string itemId)
        {
           if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(itemId))
            {
                try
                {
                    Stopwatch swMain = Stopwatch.StartNew();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                        var deleteUserProductPreferenceEndPoints = SalesforceConfigurationContext?.DeleteUserProductPreferenceEndPoints(itemId);
                        InfoLogger.Log(deleteUserProductPreferenceEndPoints, this.GetType().Name);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        Stopwatch sw = Stopwatch.StartNew();
                        var result = client.DeleteAsync(deleteUserProductPreferenceEndPoints).Result;
                        Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-DeleteUserProductPreference-Time", sw, "SalesforceAPICall");
                        Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-DeleteUserProductPreference-Time", swMain, "SalesforceOuterCall");
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
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Delete User Product Preference", e);
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
        public ISavedDocumentWriteResult DeleteSavedDocument(string accessToken, string itemId)
        {
            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(itemId))
            {
                try
                {
                    Stopwatch swMain = Stopwatch.StartNew();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                        var deleteUserProductPreferenceEndPoints = SalesforceConfigurationContext?.DeleteUserProductPreferenceEndPoints(itemId);
                        InfoLogger.Log(deleteUserProductPreferenceEndPoints, this.GetType().Name);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        Stopwatch sw = Stopwatch.StartNew();
                        var result = client.DeleteAsync(deleteUserProductPreferenceEndPoints).Result;
                        Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-DeleteSavedDocument-Time", sw, "SalesforceAPICall");
                        Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-DeleteSavedDocument-Time", swMain, "SalesforceOuterCall");
                        InfoLogger.Log(result.ReasonPhrase, this.GetType().Name);
                        if (result.IsSuccessStatusCode)
                        {
                            return new SavedDocumentWriteResult
                            {
                                Success = true,
                                Message = string.Empty
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.Log("ID&E Salesforce - Call : Delete Saved Document", e);
                    return new SavedDocumentWriteResult
                    {
                        Success = false,
                        Message = "Invalid input has been provided."
                    };
                }
            }
            return new SavedDocumentWriteResult
            {
                Success = false,
                Message = "Invalid input has been provided."
            };
        }
        public IContentResponse DeleteUserProductPreferences(string userName, string accessToken, string publicationCode, ProductPreferenceType type)
        {
            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrWhiteSpace(accessToken)
               && !string.IsNullOrWhiteSpace(publicationCode) && type != ProductPreferenceType.None)
            {
                try
                {
                    Stopwatch swMain = Stopwatch.StartNew();
                    var query = SalesforceDeleteUserProductPreferencesQueryFactory.Create(
                            userName, publicationCode, type);
                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(SalesforceConfigurationContext.SalesForceConfiguration?.Salesforce_Custom_Api_Url?.Url);

                            var deleteUserProductPreferencesEndPoints = SalesforceConfigurationContext?.DeleteUserProductPreferencesEndPoints(query);
                            InfoLogger.Log(deleteUserProductPreferencesEndPoints, this.GetType().Name);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            Stopwatch sw = Stopwatch.StartNew();
                            var result = client.DeleteAsync(deleteUserProductPreferencesEndPoints).Result;
                            Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-deleteUserProductPreferencesEndPoints-Time", sw, "SalesforceAPICall");
                            Informa.Library.Utilities.Extensions.StringExtensions.WriteSitecoreLogs("Salesforce-deleteUserProductPreferencesEndPoints-Time", swMain, "SalesforceOuterCall");
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
                    ErrorLogger.Log("ID&E Salesforce - Call : Delete User Product Preferences", e);
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

    }
}
