using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using log4net;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class CompaniesCarouselViewModel : GlassViewModel<ICompany_Landing_Page>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;
        private readonly ILog _logger;
        protected readonly IGlobalSitecoreService GlobalService;

        public CompaniesCarouselViewModel(ISiteRootContext siteRootContext,
        ITextTranslator textTranslator, ILog logger, IGlobalSitecoreService globalService)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            _logger = logger;
            GlobalService = globalService;
        }
        /// <summary>
        /// Selected Companies for Carousel 
        /// </summary>
        public IEnumerable<ICompany_Detail_Page> Companies => GlassModel?.Companies;

        /// <summary>
        /// Carousel panels
        /// </summary>                    
        public IList<CompanyModel> CompanyCarouselPanels => GetCompanyCarouselPanels();

        /// <summary>
        /// Carousel Heading
        /// </summary>
        public string CarouselHeading => TextTranslator.Translate("CompanyLandingPage.CarouselHeading");

        /// <summary>
        /// View More info text
        /// </summary>
        public string ViewMoreInfoText => TextTranslator.Translate("CompanyLandingPage.ViewMoreInfoText");

        /// <summary>
        /// Method for fetching carousel panels
        /// </summary>
        /// <returns>Carousel Panels</returns>
        private IList<CompanyModel> GetCompanyCarouselPanels()
        {
            IList<CompanyModel> SelectedCompanies = new List<CompanyModel>();

            if (Companies != null && Companies.Any())
            {
                foreach (var company in Companies)
                {
                    SelectedCompanies.Add(new CompanyModel
                    {
                        CompanyId = company.CompanyID,
                        CompanyName = company.Companyname,
                        CompanyLogo = company.Company_Logo?.Src,
                        CompanyLandingPageUrl = company._AbsoluteUrl,
                        CompanyGraphPageUrl = company.CompanyGraphDetailPage?.Url +"?Id="+ company._Id,
                        Graphs = AMGraphModels(company)
                    });
                }
            }
            return SelectedCompanies;
        }

        /// <summary>
        /// Method for selecting the graphs per company
        /// </summary>
        /// <param name="company">company</param>
        /// <returns>AM Graphs</returns>
        public IEnumerable<AMGraph> AMGraphModels(ICompany_Detail_Page company)
        {
            try
            {
                var AMGraphModels = company.Graphs.Take(3).Select(Graph => new AMGraph
                {
                    GraphID = Graph.GraphID,
                    GraphTitle = Graph.GraphTitle,
                    GraphType = !string.IsNullOrEmpty(Graph.GraphType) ? Graph.GraphType : "line",
                    GraphColor = FetchColorCode(Graph),
                    FinanceResult = FetchFinanceResults(Graph, company.CompanyID)
                });
                return AMGraphModels;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in building graph view model", ex);
                return new List<AMGraph>();
            }
        }
        /// <summary>
        /// Method for fetching color code
        /// </summary>
        /// <param name="Graph">Graph</param>
        /// <returns>color code</returns>
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
        /// Reading Graph feed data hosted in external source
        /// </summary>
        /// <param name="Graph">Graph</param>
        /// <param name="CompanyID">Company ID</param>
        /// <returns>read JSON data</returns>
        private string FetchFinanceResults(ICompany_Graph Graph, string CompanyID)
        {
            if (!string.IsNullOrWhiteSpace(Graph.GraphFeedUrl))
            {
                string feedUrl = Graph.GraphFeedUrl.Replace("{ID}", CompanyID?.Trim());
                string Content = DownloadFinancialResults(feedUrl);
                return Content;
            }
            return string.Empty;
        }
        /// <summary>
        /// Downloading feed data using feed URL
        /// </summary>
        /// <param name="feedUrl">Feed URL</param>
        /// <returns>Downloaded JSON data</returns>
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
                _logger.Error("Error in downloading the finance results from: " + feedUrl, ex);
                return string.Empty;
            }
        }
    }
}