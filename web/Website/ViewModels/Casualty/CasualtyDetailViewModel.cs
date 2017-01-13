using Informa.Library.Services.ExternalFeeds;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
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
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public readonly ICallToActionViewModel CallToActionViewModel;
        public CasualtyDetailViewModel(ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService,
            IAuthenticatedUserContext authenticatedUserContext,
            ICallToActionViewModel callToActionViewModel)
        {
            CompanyResultService = companyResultService;
            feedUrl = renderingParametersService.GetCurrentRendering().Parameters["feedurl"];
            AuthenticatedUserContext = authenticatedUserContext;
            CallToActionViewModel = callToActionViewModel;
        }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        /// <summary>
        /// External Fee URL
        /// </summary>
        public string feedUrl { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title => GlassModel?.Title;
        /// <summary>
        /// Get Casualty data from external feed url
        /// </summary>
        public string jsonCasualtyDetailData => GetCasualtyDetailData(feedUrl);

        private string GetCasualtyDetailData(string feedUrl)
        {
            string incidentId = HttpContext.Current.Request.QueryString["incidentId"];
            if (!string.IsNullOrEmpty(feedUrl) && !string.IsNullOrEmpty(incidentId))
            {
                return CompanyResultService.GetCompanyFeeds(feedUrl + "?incidentId=" + incidentId).Result;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}