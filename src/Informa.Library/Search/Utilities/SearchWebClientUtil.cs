using System;
using System.Net;
using Informa.Library.Rss;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using System.Net.Http;

namespace Informa.Library.Search.Utilities
{
    public class SearchWebClientUtil
    {
        private static string WebClientAuthUsernameSetting = "WebClientAuth.Username";
        private static string WebClientAuthPasswordSetting = "WebClientAuth.Password";
        private static string WebClientAuthDomainSetting = "WebClientAuth.Domain";
        private static HttpClient client = null;
        private static readonly object padlock = new object();
        public static HttpClient HttpClientInstance
        {
            get
            {
                lock (padlock)
                {
                    if (client == null)
                    {
                        if (AuthenticateWebClientApiCall())
                        {
                            var handler = new HttpClientHandler { Credentials = new NetworkCredential(Settings.GetSetting(WebClientAuthUsernameSetting), Settings.GetSetting(WebClientAuthPasswordSetting), Settings.GetSetting(WebClientAuthDomainSetting)) };
                            client = new HttpClient(handler);
                        }
                        else
                        {
                            client = new HttpClient();
                        }
                    }
                    return client;
                }
            }
        }

        public static SearchResults GetSearchResultsFromApi(string url)
        {
           try
            {
               ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
               return JsonConvert.DeserializeObject<SearchResults>(HttpClientInstance.GetStringAsync(url).Result);
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