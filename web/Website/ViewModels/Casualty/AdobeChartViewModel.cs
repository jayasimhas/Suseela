﻿
using Glass.Mapper.Sc;
using Informa.Library.Services.ExternalFeeds;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Informa.Web.ViewModels.FinanceCompanies;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Jabberwocky.Glass.Models;
using log4net;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class AdobeChartViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ICompaniesResultService CompanyResultService;
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly IGlobalSitecoreService GlobalService;
        private readonly ILog _logger;
        public IExternal_Feed_Url_Configuration feedUrlConfiguration { get; set; }
        protected IGlassBase Datasource;
        public AdobeChartViewModel(IGlobalSitecoreService globalService, ICompaniesResultService companyResultService,
         IAuthenticatedUserContext authenticatedUserContext, ISitecoreContext sitecoreContext, IRenderingContextService renderingParametersService, IGlassBase datasource, ILog logger)
        {
            GlobalService = globalService;
            CompanyResultService = companyResultService;
            AuthenticatedUserContext = authenticatedUserContext;
            SitecoreContext = sitecoreContext;
            feedUrlConfiguration = renderingParametersService.GetCurrentRenderingParameters<IExternal_Feed_Url_Configuration>();
            Datasource = datasource;
            _logger = logger;
        }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public AMGraph GraphDetail => GetGraph();

        private ICompany_Graph GetDataSourceItem()
        {
            if (Datasource == null)
                return null;
            var dataSourceItem = SitecoreContext.GetItem<ICompany_Graph>(Datasource._Id);
            return dataSourceItem;
        }
        
        private AMGraph GetGraph()
        {
            ICompany_Graph graph = GetDataSourceItem();
            if (graph == null)
            {
                return null;
            }
            var graphObj = new AMGraph
            {
                GraphColor = FetchColorCode(graph),
                GraphTitle = graph.GraphTitle,
                GraphType = !string.IsNullOrEmpty(graph.GraphType) ? graph.GraphType : "line",
                GraphID = graph.GraphID,
                FinanceResult = FetchFinanceResults(graph)
            };
            return graphObj;
        }

        private string FetchFinanceResults(ICompany_Graph Graph)
        {
            if (!string.IsNullOrWhiteSpace(Graph.GraphFeedUrl))
            {
                try
                {
                    string stockResult = CompanyResultService.GetCompanyFeeds(Graph.GraphFeedUrl).Result;
                    stockResult = stockResult.Replace(System.Environment.NewLine, "|");
                    return stockResult;
                }
                catch (Exception ex)
                {
                    _logger.Error("Error in downloading the finance results", ex);
                    return string.Empty;
                }
            }
            return string.Empty;
        }

     

        private string FetchColorCode(ICompany_Graph Graph)
        {
            if (Graph.GraphColor != null)
            {
                Item colorItem = GlobalService.GetItem<Item>(Graph.GraphColor);
                return colorItem.Fields["Color Code"].Value;
            }
            return string.Empty;
        }

    }

}