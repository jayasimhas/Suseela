using Informa.Library.Services.ExternalFeeds;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class CasualtyDetailViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly ICompaniesResultService CompanyResultService;
        public readonly ICallToActionViewModel CallToActionViewModel;
        protected readonly ICacheProvider CacheProvider;

        public CasualtyDetailViewModel(ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService,
            IAuthenticatedUserContext authenticatedUserContext,
            ICallToActionViewModel callToActionViewModel, ICacheProvider cacheProvider)
        {
            CompanyResultService = companyResultService;
            CallToActionViewModel = callToActionViewModel;
            feedUrlConfigurationItem = renderingParametersService.GetCurrentRenderingParameters<IExternal_Feed_Url_Configuration>();
            CacheProvider = cacheProvider;
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
        string incidentId = HttpContext.Current.Request.QueryString["incidentId"];
        private string GetCasualtyDetailData()
        {
            
            if (feedUrlConfigurationItem != null && !string.IsNullOrEmpty(feedUrlConfigurationItem.External_Feed_URL) && !string.IsNullOrEmpty(incidentId))
            {
                string cachekey = $"CasualityDetailView_{incidentId}";
                return CacheProvider.GetFromCache(cachekey, new TimeSpan(12, 0, 0), () => GetCasualtyDetailDataFeed());
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Method to fetch casualty data from external feed
        /// </summary>
        /// <returns></returns>
        private string GetCasualtyDetailDataFeed()
        {
            if (feedUrlConfigurationItem != null && !string.IsNullOrEmpty(feedUrlConfigurationItem.External_Feed_URL) && !string.IsNullOrEmpty(incidentId))
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        return client.DownloadString(string.Format(feedUrlConfigurationItem.External_Feed_URL, incidentId));
                    }
                }
                catch (Exception ex)
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