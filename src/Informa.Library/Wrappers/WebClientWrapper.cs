using System.Net;
using Jabberwocky.Autofac.Attributes;
using System.Configuration;
using System;

namespace Informa.Library.Wrappers
{
    public interface IWebClientWrapper
    {
        string DownloadString(string url);
    }

    [AutowireService]
    public class WebClientWrapper : IWebClientWrapper
    {
        public string DownloadString(string url)
        {
            using (var client = new WebClient())
            {
                string disableSSLCertificateValidation = ConfigurationManager.AppSettings["DisableSSLCertificateValidation"];
                if (!string.IsNullOrWhiteSpace(disableSSLCertificateValidation) && Convert.ToBoolean(disableSSLCertificateValidation))
                {
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                }
                return client.DownloadString(url);
            }
        }
    }
}