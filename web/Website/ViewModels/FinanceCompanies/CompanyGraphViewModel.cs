using log4net;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Informa.Library.Services.Global;
using Sitecore.Data.Items;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class CompanyGraphViewModel : GlassViewModel<ICompany_Graph_Detail_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        public ISitecoreContext SitecoreContext;
        private readonly ILog _logger;
        protected readonly IGlobalSitecoreService GlobalService;

        public CompanyGraphViewModel(ITextTranslator textTranslator, ISitecoreContext sitecoreContext, ILog logger, IGlobalSitecoreService globalService)
        {
            TextTranslator = textTranslator;
            SitecoreContext = sitecoreContext;
            _logger = logger;
            GlobalService = globalService;
        }
        /// <summary>
        /// Page Title
        /// </summary>
        public string PageTitle => GetPageTitle();
        public string ExpandAll => TextTranslator.Translate("Company.ExpandAll");
        public string CollapseAll => TextTranslator.Translate("Company.CollapseAll");
        public IEnumerable<ICompany_Graph> AMGraphs => GetAvailableGraphs();
        public string AvailableGraphsText => TextTranslator.Translate("Company.AvailableGraphs");
        /// <summary>
        /// Method for fetching Page Title
        /// </summary>
        /// <returns>Page Title</returns>
        private string GetPageTitle()
        {
            string id = HttpContext.Current.Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(id))
            {
                var compnayPage = SitecoreContext.GetItem<ICompany_Detail_Page>(id);
                if (compnayPage != null)
                {
                    CompanyID = compnayPage.CompanyID;
                    return compnayPage.Companyname;
                }
                else
                    return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// Gets available graphs for the given company_ID.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ICompany_Graph> GetAvailableGraphs()
        {
            string id = HttpContext.Current.Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(id))
            {
                var compnayPage = SitecoreContext.GetItem<ICompany_Detail_Page>(id);
                if (compnayPage != null)
                {
                    CompanyID = compnayPage.CompanyID;
                    return compnayPage.Graphs;
                }
                else
                {
                    var environmentGlobalItem = GlobalService.GetVerticalRootAncestor(GlassModel._Id)?._ChildrenWithInferType.OfType<IEnvironment_Global_Root>().FirstOrDefault();
                    CompanyID = HttpContext.Current.Request.QueryString["companyid"];
                    if (environmentGlobalItem != null)
                    {
                        var Graphsfolder = SitecoreContext.GetItem<Item>(environmentGlobalItem._Id)?.Children.FirstOrDefault(p => p.Name.Equals("Company Graph Types"));
                        if(Graphsfolder != null)
                        {
                            var Graphs = (from graph in Graphsfolder.Children
                                          select SitecoreContext.GetItem<ICompany_Graph>(graph.ID.Guid));
                            return Graphs;
                        }
                        return Enumerable.Empty<ICompany_Graph>();
                    }
                    return Enumerable.Empty<ICompany_Graph>();
                }
                    
            }
            else
            {
                return Enumerable.Empty<ICompany_Graph>();
            }
        }
        public string CompanyID { get; set; }

        public IEnumerable<AMGraph> AMGraphModels
        {
            get
            {
                try
                {
                    var AMGraphModels = AMGraphs.Select(Graph => new AMGraph
                    {
                        GraphID = Graph.GraphID,
                        GraphTitle = Graph.GraphTitle,
                        GraphType = !string.IsNullOrEmpty(Graph.GraphType) ? Graph.GraphType : "line",
                        GraphColor = FetchColorCode(Graph),
                        FinanceResult = FetchFinanceResults(Graph)

                    });
                    return AMGraphModels;

                }
                catch (Exception ex)
                {
                    _logger.Error("Error in building graph detail view model", ex);
                    return new List<AMGraph>();
                }

            }
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

        private string FetchFinanceResults(ICompany_Graph Graph)
        {
            if (!string.IsNullOrWhiteSpace(Graph.GraphFeedUrl))
            {
                string feedUrl = Graph.GraphFeedUrl.Replace("{ID}", CompanyID?.Trim());
                string Content = DownloadFinancialResults(feedUrl);
                return Content;
            }
            return string.Empty;
        }

        private string DownloadFinancialResults(string feedUrl)
        {
            try
            {
                var client = new WebClient();
                var content = client.DownloadString(feedUrl);
                return content;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in downloading the finance results", ex);
                return string.Empty;
            }
        }
    }
}