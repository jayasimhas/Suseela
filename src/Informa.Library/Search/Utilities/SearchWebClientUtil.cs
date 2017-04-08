using System;
using System.Net;
using Informa.Library.Rss;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Informa.Library.Search.Utilities
{
    public class SearchWebClientUtil
    {
        private static string WebClientAuthUsernameSetting = "WebClientAuth.Username";
        private static string WebClientAuthPasswordSetting = "WebClientAuth.Password";
        private static string WebClientAuthDomainSetting = "WebClientAuth.Domain";

        public static SearchResults GetSearchResultsFromApi(string url)
        {
            try
            {
                using (var client = new WebClient())
                {                   
                    return JsonConvert.DeserializeObject<SearchResults>(client.DownloadString(url));
                }

            }
            catch (Exception exc)
            {
                Log.Error("Could not retrieve search results for: " + url + " : " + exc.Message,
                    "GetSearchResultsFromApi");
                return null;
            }
        }
        public static bool AuthenticateWebClientApiCall()
        {
            return (!string.IsNullOrEmpty(Settings.GetSetting(WebClientAuthUsernameSetting)) &&
                    !string.IsNullOrEmpty(Settings.GetSetting(WebClientAuthPasswordSetting)) &&
                    !string.IsNullOrEmpty(Settings.GetSetting(WebClientAuthDomainSetting)));
        }
    }
}