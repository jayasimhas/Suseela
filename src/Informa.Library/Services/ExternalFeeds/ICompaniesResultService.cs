namespace Informa.Library.Services.ExternalFeeds
{
    using System.Threading.Tasks;

    public interface ICompaniesResultService
    {
        Task<string> GetCompanyFeeds(string feedUrl);
    }
}
