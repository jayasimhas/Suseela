
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
    public class RelatedLinksViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ICompaniesResultService CompanyResultService;
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        public RelatedLinksViewModel(ICompaniesResultService companyResultService,
           IAuthenticatedUserContext authenticatedUserContext, ISiteRootContext siteRootContext)
        {
            CompanyResultService = companyResultService;
            AuthenticatedUserContext = authenticatedUserContext;
            SiteRootContext = siteRootContext;
        }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public List<IMarketData_Folder> marketDataFolders => GetRelatedFolders();

        private List<IMarketData_Folder> GetRelatedFolders()
        {
            List<IMarketData_Folder> marketDataFolders = new List<IMarketData_Folder>();
            marketDataFolders = GlassModel._ChildrenWithInferType.OfType<IMarketData_Folder>()?.ToList();
            if(marketDataFolders != null && marketDataFolders.Any())
                return marketDataFolders;
            else
            {
                if(GlassModel is IMarketData_Detail_Page)
                {
                    marketDataFolders = new List<IMarketData_Folder>();
                    marketDataFolders.Add(GlassModel._Parent._Parent._ChildrenWithInferType.OfType<IMarketData_Folder>().FirstOrDefault(p => p._Id.Equals(GlassModel._Parent._Id)));
                }
               return marketDataFolders;
            }
        }
    }
}