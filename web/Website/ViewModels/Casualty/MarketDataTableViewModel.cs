using Informa.Library.Services.ExternalFeeds;
using Informa.Library.Services.Global;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Jabberwocky.Glass.Models;
using System;
using System.Net;

namespace Informa.Web.ViewModels.Casualty
{
    public class MarketDataTableViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ICompaniesResultService CompanyResultService;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ICacheProvider CacheProvider;
        public MarketDataTableViewModel(ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService,
            IAuthenticatedUserContext authenticatedUserContext,
            IGlobalSitecoreService globalService, ICacheProvider cacheProvider)
        {
            CompanyResultService = companyResultService;
            GlobalService = globalService;
            RenderingParameters = renderingParametersService.GetCurrentRenderingParameters<IMarket_Data_Table_Types_With_FeedLink>();
            CacheProvider = cacheProvider;
        }
        //Table Variation-1
        public IMarket_Data_Table_Types_With_FeedLink RenderingParameters { get; set; }
        

        /// <summary>
        /// Baltic indices data in json format
        /// </summary>
        public string jsonTableData => GetTablesData();

        private string GetTablesData()
        {
            if (RenderingParameters != null && !string.IsNullOrEmpty(RenderingParameters.External_Feed_URL))
            {
                string cachekey = $"MarketdataTableViewmodel_{ RenderingParameters.External_Feed_URL}";
                return CacheProvider.GetFromCache(cachekey, new TimeSpan(12, 0, 0), () => GetTablesDataFromFeed());
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Method to get Baltic Indices data from external url
        /// </summary>
        /// <returns></returns>
        private string GetTablesDataFromFeed()
        {
            if (RenderingParameters != null && !string.IsNullOrEmpty(RenderingParameters.External_Feed_URL))
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        return client.DownloadString(RenderingParameters.External_Feed_URL);
                    }
                }
                catch(Exception ex)
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
    }
}