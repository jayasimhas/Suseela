using Informa.Library.Services.ExternalFeeds;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Jabberwocky.Glass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class BalticIndicesViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ICompaniesResultService CompanyResultService;       
        public BalticIndicesViewModel(ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            CompanyResultService = companyResultService;            
            feedUrlConfigurationItem = renderingParametersService.GetCurrentRenderingParameters<IExternal_Feed_Url_Configuration>();
        }
                
        /// <summary>
        /// External Fee URL
        /// </summary>
        public IExternal_Feed_Url_Configuration feedUrlConfigurationItem { get; set; }
        /// <summary>
        /// Baltic indices data in json format
        /// </summary>
        public string jsonBalticIndices => GetBalticIndicesData();
        /// <summary>
        /// Method to get Baltic Indices data from external url
        /// </summary>
        /// <returns></returns>
        private string GetBalticIndicesData()
        {
            return feedUrlConfigurationItem != null && !string.IsNullOrEmpty(feedUrlConfigurationItem.External_Feed_URL) ? CompanyResultService.GetCompanyFeeds(feedUrlConfigurationItem.External_Feed_URL).Result : string.Empty;
        }
    }
}