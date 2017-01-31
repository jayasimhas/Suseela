using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using log4net;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class PeerGroupDetailViewModel : GlassViewModel<ICompany_Peer_Group_Detail_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        private ISitecoreContext SitecoreContext;
        private readonly ILog _logger;
        private readonly IGlobalSitecoreService GlobalService;

        public PeerGroupDetailViewModel(ITextTranslator textTranslator, ISitecoreContext sitecoreContext, ILog logger, IGlobalSitecoreService globalService)
        {
            TextTranslator = textTranslator;
            SitecoreContext = sitecoreContext;
            _logger = logger;
            GlobalService = globalService;
        }

        /// <summary>
        /// Page Title
        /// </summary>
        public string PeerGroupPageTitle => GetPageTitle();
        /// <summary>
        /// Dictionary Item for Page Title
        /// </summary>
        public string PeerGroupPageTitleTemplate => TextTranslator.Translate("PeerGroup.DetailPage.Title");
        //public List<string> companyList { get; set; }
        public string ExpandAll => TextTranslator.Translate("Company.ExpandAll");
        public string CollapseAll => TextTranslator.Translate("Company.CollapseAll");
        public string AvailableGraphsText => TextTranslator.Translate("Company.AvailableGraphs");

        public List<PeerCompanyGraph> PeerCompaniesGraphList => GetPeerCompaniesGraphList();

        //public string CompanyCompareColors => GetCompareColors();
        /// <summary>
        /// Method for fetching Peer Page Title
        /// </summary>
        /// <returns></returns>
        private string GetPageTitle()
        {
            string id = HttpContext.Current.Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(id))
            {
                var compnayPage = SitecoreContext.GetItem<ICompany_Detail_Page>(id);
                return compnayPage.Companyname;
            }
            else
            {
                return string.Empty;
            }
            
        }
        private IEnumerable<ICompany_Detail_Page> PeerCompanyPages { get; set; }
        /// <summary>
        /// Gets Peer group Companies along with corresponding Graphs
        /// </summary>
        /// <returns></returns>
        private List<PeerCompanyGraph> GetPeerCompaniesGraphList()
        {
            List<PeerCompanyGraph> PeerCompanyGraphList = new List<PeerCompanyGraph>();
            List<ICompany_Detail_Page> PeerGroupList = GetCompanyPeerGroupList();
            PeerCompanyPages = PeerGroupList;
            if (PeerGroupList != null && PeerGroupList.Any())
            {
                var AMGraphs = PeerGroupList[0].Graphs;
                if (AMGraphs != null && AMGraphs.Any())
                {
                    foreach (var graph in AMGraphs)
                    {
                        PeerCompanyGraph peerCompanyGraph = new PeerCompanyGraph();
                        peerCompanyGraph.GraphID = graph.GraphID;
                        peerCompanyGraph.GraphName = graph.GraphTitle;
                        peerCompanyGraph.CompanyGraphs = GetPeerCompamniesGraphModels(graph);
                        PeerCompanyGraphList.Add(peerCompanyGraph);
                    }
                }
                
                return PeerCompanyGraphList;
            }
            return PeerCompanyGraphList;
        }
        /// <summary>
        /// Gets context peergroup companies
        /// </summary>
        /// <returns></returns>
        private List<ICompany_Detail_Page> GetCompanyPeerGroupList()
        {
            string id = HttpContext.Current.Request.QueryString["Id"];
            List<ICompany_Detail_Page> peerGroupList = new List<ICompany_Detail_Page>();
            if (!string.IsNullOrEmpty(id))
            {
                var companyPage = SitecoreContext.GetItem<ICompany_Detail_Page>(id);
                if (companyPage != null)
                {
                    peerGroupList.Add(companyPage);
                    var peerCompanies = companyPage.Company_PeerGroupList as IEnumerable<ICompany_Detail_Page>;
                    if (peerCompanies != null)
                    {
                        peerCompanies = peerCompanies.Where(p => p._Id != companyPage._Id);
                        peerGroupList.AddRange(peerCompanies);
                    }
                    return peerGroupList;
                }
                else
                    return peerGroupList;
            }
            else
            {
                return peerGroupList;
            }
        }

        /// <summary>
        /// Gets Given graphs settings for all the peer groups
        /// </summary>
        /// <param name="Graph"></param>
        /// <returns></returns>
        private List<PeerCompany> GetPeerCompamniesGraphModels(ICompany_Graph Graph)
        {
            try
            {
                List<PeerCompany> peerCompanyDataList = new List<PeerCompany>();
                if (PeerCompanyPages != null && PeerCompanyPages.Any())
                {
                    peerCompanyDataList = PeerCompanyPages.Select(company => new PeerCompany
                    {
                        CompanyID = company.CompanyID,
                        CompanyName = company.Companyname,
                        FinanceResult = FetchFinanceResults(Graph, company.CompanyID)

                    }).ToList();
                    return peerCompanyDataList;
                }
                return peerCompanyDataList;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in building graph view model in PeerGroupDetail View Model", ex);
                return new List<PeerCompany>();
            }

        }
        /// <summary>
        /// Fetches Graph color
        /// </summary>
        /// <param name="Graph"></param>
        /// <returns></returns>
        private string FetchColorCode(ICompany_Graph Graph)
        {
            if (Graph.GraphColor != null)
            {
                Item colorItem = GlobalService.GetItem<Item>(Graph.GraphColor);
                return colorItem.Fields["Color Code"].Value;
            }
            return string.Empty;
        }
        /// <summary>
        /// Gets financial results from the feed service
        /// </summary>
        /// <param name="Graph"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        private string FetchFinanceResults(ICompany_Graph Graph, string companyID)
        {
            if (!string.IsNullOrWhiteSpace(Graph.GraphFeedUrl))
            {
                string feedUrl = Graph.GraphFeedUrl.Replace("{ID}", companyID?.Trim());
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

        //private string GetCompareColors()
        //{
        //    string GraphCompareColorsFolderId = new ItemReferences().GraphCompareColorsFolder.ToString();
        //    List<string> sb = new List<string>();
        //    if(GraphCompareColorsFolderId != null)
        //    {
        //        Item colorItemFolder = GlobalService.GetItem<Item>(GraphCompareColorsFolderId);
        //        if(colorItemFolder !=null && colorItemFolder.Children != null)
        //        {
        //            var colorChildren = colorItemFolder.Children.Take(PeerCompanyPages.Count());
        //            foreach (var color in colorChildren)
        //            {
        //                sb.Add(color.Fields["Color Code"].Value);
        //            }

        //        }
        //        return string.Join(",", sb);
        //    }
        //    return string.Empty;
        //}

    }
}