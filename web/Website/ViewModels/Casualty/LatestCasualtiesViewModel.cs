using Glass.Mapper.Sc.Fields;
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
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public LatestCasualtiesViewModel(ISiteRootContext siterootContext, 
            ICompaniesResultService companyResultService,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            SiterootContext = siterootContext;
            CompanyResultService = companyResultService;
            AuthenticatedUserContext = authenticatedUserContext;
        }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public string homePageUrl => SiterootContext?.Item._Url;
        public Image Logo => GlassModel?.Logo;
        public string Title => GlassModel?.Title;
        public string AdditionalInformation => GlassModel?.Additional_Information;
        public string FeedUrl => GlassModel?.External_Feed_Url;
        public string jsonLatestCasualties => GetLatestCasualties();

        private string GetLatestCasualties()
        {
            return !string.IsNullOrEmpty(FeedUrl) ? CompanyResultService.GetCompanyFeeds(FeedUrl).Result : string.Empty;
        }
    }
}