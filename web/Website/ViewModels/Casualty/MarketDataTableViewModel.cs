using Informa.Library.Services.ExternalFeeds;
using Informa.Library.Services.Global;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Jabberwocky.Glass.Models;
using System.Net;

namespace Informa.Web.ViewModels.Casualty
{
    public class MarketDataTableViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ICompaniesResultService CompanyResultService;
        protected readonly IGlobalSitecoreService GlobalService;
        public MarketDataTableViewModel(ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService,
            IAuthenticatedUserContext authenticatedUserContext,
            IGlobalSitecoreService globalService)
        {
            CompanyResultService = companyResultService;
            GlobalService = globalService;
            RenderingParameters = renderingParametersService.GetCurrentRenderingParameters<IMarket_Data_Table_Types_With_FeedLink>();
        }
        //Table Variation-1
        public IMarket_Data_Table_Types_With_FeedLink RenderingParameters { get; set; }
        /// <summary>
        /// Baltic indices data in json format
        /// </summary>
        public string jsonTableData => GetTablesData();
        /// <summary>
        /// Method to get Baltic Indices data from external url
        /// </summary>
        /// <returns></returns>
        private string GetTablesData()
        {
            if (RenderingParameters != null && !string.IsNullOrEmpty(RenderingParameters.External_Feed_URL))
            {
                var client = new WebClient();
                return client.DownloadString(RenderingParameters.External_Feed_URL);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}