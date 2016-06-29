using System.Net;
using Jabberwocky.Autofac.Attributes;

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
                return client.DownloadString(url);
            }
        }
    }
}