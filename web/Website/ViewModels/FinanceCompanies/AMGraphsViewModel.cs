using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using log4net;
using Informa.Library.Services.Global;
using Sitecore.Data.Items;
using System.Net;
using Newtonsoft.Json;
using Glass.Mapper.Sc;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class AMGraphsViewModel : GlassViewModel<ICompany_Detail_Page>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;
        private readonly ILog _logger;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISitecoreContext SitecoreContext;

        public AMGraphsViewModel(ISiteRootContext siteRootContext,
        ITextTranslator textTranslator, ILog logger, IGlobalSitecoreService globalService, ISitecoreContext sitecoreContext)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            _logger = logger;
            GlobalService = globalService;
            SitecoreContext = sitecoreContext;
        }

        public string CompanyName => GlassModel.Companyname;

        public string FiveYearTrends => TextTranslator.Translate("Company.FiveYearTrends");
        public string CompanyID => GlassModel.CompanyID;
        public string CompanyGraphPageUrl => GlassModel?.CompanyGraphDetailPage?.Url;        
        public IEnumerable<ICompany_Graph> AMGraphs => GlassModel.Graphs;

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
                    _logger.Error("Error in building graph view model", ex);
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

        //private List<ICompany_Detail_Page> GetCompanyPeerGroupList()
        //{
        //    string id = HttpContext.Current.Request.QueryString["Id"];
        //    List<ICompany_Detail_Page> peerGroupList = new List<ICompany_Detail_Page>();
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var companyPage = SitecoreContext.GetItem<ICompany_Detail_Page>(id);
        //        if (companyPage != null)
        //        {
        //            peerGroupList.Add(companyPage);
        //            var peerCompanies = companyPage.Company_PeerGroupList;
        //            if (peerCompanies != null)
        //            {

        //            }
        //            peerGroupList.AddRange(peerCompanies.ToList<ICompany_Detail_Page>());
        //        }
        //        else
        //            return string.Empty;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}
    }
}