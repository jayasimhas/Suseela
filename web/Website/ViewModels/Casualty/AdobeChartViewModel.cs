
using Informa.Library.Services.ExternalFeeds;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.MarketData;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Jabberwocky.Glass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class AdobeChartViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ICompaniesResultService CompanyResultService;
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        public AdobeChartViewModel(ICompaniesResultService companyResultService,
           IAuthenticatedUserContext authenticatedUserContext, ISiteRootContext siteRootContext)
        {
            CompanyResultService = companyResultService;
            AuthenticatedUserContext = authenticatedUserContext;
            SiteRootContext = siteRootContext;
        }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
       
    }
}