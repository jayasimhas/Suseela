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
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Sitecore.Data.Fields;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Core.Caching;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class MergersAndAquisitionsViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;
        private readonly ILog _logger;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISitecoreContext SitecoreContext;
        protected IGlassBase Datasource;
        protected readonly ICacheProvider CacheProvider;
        private string FeedUrl { get; set; }

        public MergersAndAquisitionsViewModel(ISiteRootContext siteRootContext,
        ITextTranslator textTranslator, ILog logger, IGlobalSitecoreService globalService, ISitecoreContext sitecoreContext, IGlassBase datasource, IRenderingContextService renderingContextService, ICacheProvider cacheProvider)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            _logger = logger;
            GlobalService = globalService;
            SitecoreContext = sitecoreContext;
            Datasource = datasource;
            FeedUrl = HttpContext.Current.Server.UrlDecode(renderingContextService.GetCurrentRendering().Parameters["feedurl"]);
            CacheProvider = cacheProvider;
        }

        public IEnumerable<AMGraph> AMGraphs => GetGraphs();

        public string MergersAquitions => FetchMergersAquistionsResults();

        public string Aquirer => TextTranslator.Translate("MA.Aquirer");
        public string Dashboard => TextTranslator.Translate("MA.Dashboard");
        public string Month => TextTranslator.Translate("MA.Month");
        public string Price => TextTranslator.Translate("MA.Price");
        public string ShowLargest => TextTranslator.Translate("MA.ShowLargest");
        public string TableTitle => TextTranslator.Translate("MA.TableTitle");
        public string Target => TextTranslator.Translate("MA.Target");
        public string TargetLocation => TextTranslator.Translate("MA.TargetLocation");
        public string TargetSector => TextTranslator.Translate("MA.TargetSector");

        private IEnumerable<AMGraph> GetGraphs()
        {
            if (Datasource != null)
            {
                List<ICompany_Graph> garphList = (from graph in Datasource._ChildrenWithInferType.OfType<IGlassBase>()
                                                  select SitecoreContext.GetItem<ICompany_Graph>(graph._Id)).ToList();
                var graphItems = garphList?.Select(Graph => new AMGraph
                {
                    GraphColor = FetchColorCode(Graph),
                    GraphTitle = Graph.GraphTitle,
                    GraphType = !string.IsNullOrEmpty(Graph.GraphType) ? Graph.GraphType : "line"
                });
                return graphItems;
            }
            return Enumerable.Empty<AMGraph>();
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

        private string MergersAquistionsResults(string feedUrl)
        {
           string Content = ConvertToRequiredJsonFormat(DownloadFinancialResults(feedUrl));
           return Content;
           
        }

        private string FetchMergersAquistionsResults()
        {
            if (!string.IsNullOrWhiteSpace(FeedUrl))
            {
                string reqYear = HttpContext.Current.Request.QueryString["year"];
                reqYear = !string.IsNullOrWhiteSpace(reqYear) ? reqYear : DateTime.Now.Year.ToString();
                FeedUrl = FeedUrl + "?year=" + reqYear;
                string cacheKey = CreateCacheKey("FetchMergersAquistionsResults" + reqYear);
                string Content = CacheProvider.GetFromCache(cacheKey, () => MergersAquistionsResults(FeedUrl));
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
                _logger.Error("Error in downloading the Mergers Aquisitions results", ex);
                return string.Empty;
            }
        }

        private string ConvertToRequiredJsonFormat(string results)
        {

            if (!string.IsNullOrWhiteSpace(results))
            {
                var jsonData = JsonConvert.DeserializeObject<JToken>(results);
                if (jsonData != null)
                {
                    var mergersAquirerList = jsonData.Children<JObject>().Select(jsonResult => new MergersAquisitionsResult
                    {
                        Month = GetMonthName(jsonResult["deal_month"].Value<string>()),
                        Acquirer = ReturnCompanyPathIfExists(jsonResult["acquirer_company_id"].Value<string>(), jsonResult["acquirer_company_name"].Value<string>()),
                        Target = ReturnCompanyPathIfExists(jsonResult["target_company_id"].Value<string>(), jsonResult["target_company_name"].Value<string>()),
                        TargetSector = jsonResult["target_sector"].Value<string>(),
                        TargetLocation = jsonResult["target_location"].Value<string>(),
                        Detail = jsonResult["details"].Value<string>(),
                        Price = jsonResult["price"].Value<string>()
                    });
                    return JsonConvert.SerializeObject(mergersAquirerList);
                }
            }
            return string.Empty;
        }
        private string GetMonthName(string num)
        {
            int month;
            if (int.TryParse(num, out month))
            {
                var dateFormat = CultureInfo.CurrentCulture.DateTimeFormat;
                return dateFormat.GetMonthName(month);
            }
            return num;
        }

        private List<FinanceCompany> FinanceCompaniesList()
        {
            var contextSite = GlobalService.GetSiteRootAncestor(GlassModel._Id);
            var companyParent = contextSite._ChildrenWithInferType.OfType<IHome_Page>()?.FirstOrDefault()
                ._ChildrenWithInferType.OfType<ICompany_Landing_Page>()?.FirstOrDefault();
            var ComapnyList = companyParent?._ChildrenWithInferType.OfType<ICompany_Detail_Page>()?.ToList();
            var financeCompanyList = ComapnyList?.Select(p => new FinanceCompany
            {
                CompanyID = p.CompanyID,
                Path = "/" + companyParent._Name + "/" + p._Name + "/"
            }).ToList();
            return financeCompanyList;
        }
        private string CreateCacheKey(string suffix)
        {
            return $"{nameof(MergersAndAquisitionsViewModel)}-{suffix}";
        }

        private string ReturnCompanyPathIfExists(string companyID, string companyName)
        {
            string cacheKey = CreateCacheKey("FinanceCompaniesList");
            List<FinanceCompany> financeCompanyList =  CacheProvider.GetFromCache(cacheKey,()=> FinanceCompaniesList());
            if(financeCompanyList != null &&  financeCompanyList.Any())
            {
                var matchCompany = financeCompanyList.Where(c => c.CompanyID.Equals(companyID))?.FirstOrDefault();
                if (matchCompany != null)
                    return "<a href='"+ matchCompany.Path + "'>" + companyName + "</a>";
                else
                    return companyName;
            }
            return companyName;
        }

    }
}