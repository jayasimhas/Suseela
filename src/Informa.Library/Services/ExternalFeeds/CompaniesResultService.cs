namespace Informa.Library.Services.ExternalFeeds
{
    using Jabberwocky.Autofac.Attributes;
    using System.Net.Http;
    using System.Threading.Tasks;

    [AutowireService(LifetimeScope = LifetimeScope.SingleInstance)]
    public class CompaniesResultService : ICompaniesResultService
    {
        private readonly HttpClient httpClient;

        public CompaniesResultService()
        {
            httpClient = new HttpClient();
        }
        public Task<string> GetCompanyFeeds(string feedUrl)
        {
           return httpClient.GetStringAsync(feedUrl);
        }
    }
}
