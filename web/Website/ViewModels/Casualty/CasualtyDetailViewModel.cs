using Informa.Library.Services.ExternalFeeds;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class CasualtyDetailViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly ICompaniesResultService CompanyResultService;       
        public readonly ICallToActionViewModel CallToActionViewModel;
        public CasualtyDetailViewModel(ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService,
            IAuthenticatedUserContext authenticatedUserContext,
            ICallToActionViewModel callToActionViewModel)
        {
            CompanyResultService = companyResultService;            
            CallToActionViewModel = callToActionViewModel;
            feedUrlConfigurationItem = renderingParametersService.GetCurrentRenderingParameters<IExternal_Feed_Url_Configuration>();
        }      
        /// <summary>
        /// External Fee URL
        /// </summary>
        public IExternal_Feed_Url_Configuration feedUrlConfigurationItem { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title => GlassModel?.Title;
        /// <summary>
        /// Get Casualty data from external feed url
        /// </summary>
        public string jsonCasualtyDetailData => GetCasualtyDetailData();

        /// <summary>
        /// Method to fetch casualty data from external feed
        /// </summary>
        /// <returns></returns>
        private string GetCasualtyDetailData()
        {
            string incidentId = HttpContext.Current.Request.QueryString["incidentId"];
            if (feedUrlConfigurationItem != null && !string.IsNullOrEmpty(feedUrlConfigurationItem.External_Feed_URL) && !string.IsNullOrEmpty(incidentId))
            {
                return CompanyResultService.GetCompanyFeeds(feedUrlConfigurationItem.External_Feed_URL + "?incidentId=" + incidentId).Result;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}