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
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public BalticIndicesViewModel(ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            CompanyResultService = companyResultService;
            AuthenticatedUserContext = authenticatedUserContext;
            feedUrlConfigurationItem = renderingParametersService.GetCurrentRenderingParameters<IExternal_Feed_Url_Configuration>();
        }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        /// <summary>
        /// External Fee URL
        /// </summary>
        public IExternal_Feed_Url_Configuration feedUrlConfigurationItem { get; set; }

        public string jsonBalticIndices => GetBalticIndicesData();
        private string GetBalticIndicesData()
        {
            return feedUrlConfigurationItem != null && !string.IsNullOrEmpty(feedUrlConfigurationItem.External_Feed_URL) ? CompanyResultService.GetCompanyFeeds(feedUrlConfigurationItem.External_Feed_URL).Result : string.Empty;
        }
    }
}