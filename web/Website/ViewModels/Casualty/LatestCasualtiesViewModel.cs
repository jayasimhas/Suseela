﻿using Glass.Mapper.Sc.Fields;
using Informa.Library.Services.ExternalFeeds;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class LatestCasualtiesViewModel : GlassViewModel<ILatest_Casualties_Component>
    {
        protected readonly ISiteRootContext SiterootContext;
        protected readonly ICompaniesResultService CompanyResultService;
        public LatestCasualtiesViewModel(ISiteRootContext siterootContext,
            ICompaniesResultService companyResultService,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            SiterootContext = siterootContext;
            CompanyResultService = companyResultService;
        }
        /// <summary>
        /// HomePage URL
        /// </summary>
        public string homePageUrl => SiterootContext?.Item._Url;
        /// <summary>
        /// Logo
        /// </summary>
        public Image Logo => GlassModel?.Logo;
        /// <summary>
        /// Title
        /// </summary>
        public string Title => GlassModel?.Title;
        /// <summary>
        /// Additional information
        /// </summary>
        public string AdditionalInformation => GlassModel?.Additional_Information;
        /// <summary>
        /// Feed URL
        /// </summary>
        public string FeedUrl => GlassModel?.External_Feed_Url;
        /// <summary>
        /// Casualty Detail Page Url
        /// </summary>
        public string CasualtyDetailPageUrl => GlassModel?.Casualty_Detail_Page_Link?.Url;
        /// <summary>
        /// Call to action for Logo
        /// </summary>
        public string LogoCallToAction => GlassModel?.Logo_Call_to_Action?.Url;
        /// <summary>
        /// Json Latest casualties data
        /// </summary>
        public string jsonLatestCasualties => GetLatestCasualties();

        /// <summary>
        /// Method to get latest casualties data
        /// </summary>
        /// <returns></returns>
        private string GetLatestCasualties()
        {
            try
            {
                return !string.IsNullOrEmpty(FeedUrl) ? CompanyResultService.GetCompanyFeeds(FeedUrl).Result : string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
    }
}