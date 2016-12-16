namespace Informa.Web.ViewModels.FinanceCompanies
{
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration.Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    using Library.Services.ExternalFeeds;

    public class CompaniesResultViewModel : GlassViewModel<ICompany_Table_Component>
    {
        protected readonly ICompaniesResultService companyResultService;

        public CompaniesResultViewModel(ICompaniesResultService CompaniesResultService)
        {
            companyResultService = CompaniesResultService;
        }

        public string ResultsData => !string.IsNullOrWhiteSpace(GlassModel.ExternalFeedUrl) ? 
                                     companyResultService.GetCompanyFeeds(GlassModel.ExternalFeedUrl).Result :
                                     "External feed url is empty";
       
    }
}