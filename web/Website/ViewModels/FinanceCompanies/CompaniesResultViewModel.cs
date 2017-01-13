namespace Informa.Web.ViewModels.FinanceCompanies
{
    using System;
    using Glass.Mapper.Sc;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration.Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
    using Jabberwocky.Glass.Autofac.Mvc.Models;
    using Library.Services.ExternalFeeds;
    using Sitecore.Data.Items;
    using System.Collections.Generic;
    using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
    using System.Linq;
    using Newtonsoft.Json;
    using Sitecore.Configuration;
    using Jabberwocky.Glass.Autofac.Mvc.Services;
    using System.Web;
    using Newtonsoft.Json.Linq;
    using Informa.Library.Utilities.References;

    public class CompaniesResultViewModel : GlassViewModel<ICompany_Table_Component>
    {
        protected readonly ICompaniesResultService companyResultService;
        protected readonly ISitecoreContext sitecoreContext;
        public CompaniesResultViewModel(ICompaniesResultService CompaniesResultService, ISitecoreContext context, IRenderingContextService renderingParametersService)
        {
            companyResultService = CompaniesResultService;
            sitecoreContext = context;
            RenderingParameters = renderingParametersService.GetCurrentRenderingParameters<ICompany_Results_Table_Type_Options>();
        }

        public ICompany_Results_Table_Type_Options RenderingParameters { get; set; }
        public string ResultsData => GetResultsData();
        public string CompareFinancialResults => !string.IsNullOrWhiteSpace(GlassModel.ExternalFeedUrl) ?
                                    GetFinancialResultsForCompare(companyResultService.GetCompanyFeeds(GlassModel.ExternalFeedUrl).Result) :
                                    "External feed url is empty";
        /// <summary>
        /// Method to get the companies financial results for compare 
        /// </summary>
        /// <param name="financialResults"></param>
        /// <returns></returns>
        public string GetFinancialResultsForCompare(string financialResults)
        {
            List<string> companyIds = new List<string>();
            string id = HttpContext.Current.Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(id))
            {
                var companyPage = sitecoreContext.GetItem<ICompany_Detail_Page>(id);
                companyIds.Add(!string.IsNullOrEmpty(companyPage.CompanyID) ? companyPage.CompanyID : string.Empty);
                if (companyPage.Company_PeerGroupList != null && companyPage.Company_PeerGroupList.Any())
                {
                    foreach (var peerPage in companyPage.Company_PeerGroupList)
                    {
                        if (peerPage._Id != companyPage._Id)
                        {
                            var peerCompanyPage = sitecoreContext.GetItem<ICompany_Detail_Page>(peerPage._Id);
                            companyIds.Add(!string.IsNullOrEmpty(peerCompanyPage.CompanyID) ? peerCompanyPage.CompanyID : string.Empty);

                        }
                    }
                }
                var jsonData = JsonConvert.DeserializeObject<JToken>(financialResults);
                if (jsonData != null)
                {
                    jsonData.Children<JObject>()
                        .Where(x => !companyIds.Contains(x["ID"].Value<string>()))
                        .ToList()
                        .ForEach(doc => doc.Remove());
                }
                return jsonData.ToString();
            }
            return string.Empty;
        }
        public string JsonMappingData => GetJsonMappingData();

        private string GetJsonMappingData()
        {
            string result = string.Empty;
            var graphTypeNode = sitecoreContext.GetItem<Item>(Settings.GetSetting("Company.Graph.Type"));
            List<ICompany_Graph> companyGraphItems = new List<ICompany_Graph>();

            if (graphTypeNode != null && graphTypeNode.Children != null && graphTypeNode.Children.Any())
            {
                companyGraphItems.AddRange(graphTypeNode.Children.Select(item => sitecoreContext.GetItem<ICompany_Graph>(item.ID.Guid)));

                if (companyGraphItems != null && companyGraphItems.Any())
                    result = JsonConvert.SerializeObject(companyGraphItems.Select(x => new { Key = x.GraphID, Value = x.GraphTitle }));
            }

            return result;
        }

        /// <summary>
        /// Gets Json data from the feed
        /// </summary>
        /// <returns>Json string</returns>
        public string GetResultsData()
        {
            string jsonString = "External feed url is empty";
            string companyQueryId = Convert.ToString(HttpContext.Current.Request.QueryString["companyid"]);
            if(RenderingParameters != null && RenderingParameters.TableType != null && (Sitecore.Context.Item.Fields["Company ID"] != null || !string.IsNullOrWhiteSpace(companyQueryId)) &&
                (string.Equals(RenderingParameters.TableType.Value,Constants.CompaniesResultTableTypes.FinancialResults.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(RenderingParameters.TableType.Value, Constants.CompaniesResultTableTypes.QuarterlyResults.ToString(), StringComparison.InvariantCultureIgnoreCase)))
            {
                string feedUrl = string.Format(GlassModel.ExternalFeedUrl, !string.IsNullOrWhiteSpace(companyQueryId) ? companyQueryId : Sitecore.Context.Item.Fields["Company ID"].Value);
                return companyResultService.GetCompanyFeeds(feedUrl).Result;
            }
            jsonString = companyResultService.GetCompanyFeeds(GlassModel.ExternalFeedUrl).Result;

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                var companyLandingPage = sitecoreContext.GetItem<Item>(Settings.GetSetting("Company.Landing.Page"));
                List<ICompany_Detail_Page> companyDetailPages = new List<ICompany_Detail_Page>();

                if (companyLandingPage != null && companyLandingPage.Children != null && companyLandingPage.Children.Any())
                {
                    companyDetailPages.AddRange(companyLandingPage.Children.Select(item => sitecoreContext.GetItem<ICompany_Detail_Page>(item.ID.Guid)));
                    var jsonData = JsonConvert.DeserializeObject<JToken>(jsonString);

                    if (jsonData != null && companyDetailPages != null && companyDetailPages.Count > 0 && companyDetailPages.Any())
                    {
                        var jData = jsonData.Children<JObject>();

                        var result = companyDetailPages.SelectMany(company => jData.Where(data => string.Equals(company.CompanyID, data["ID"].Value<string>()))).Distinct().ToList();
                        result.ForEach(item => item.Add("CompanyPageUrl", GetCompanyPageUrl(item, companyDetailPages)));

                        return JsonConvert.SerializeObject(result);
                    }
                }
            }

            return jsonString;
        }

        private JToken GetCompanyPageUrl(JObject item, List<ICompany_Detail_Page> companyDetailPages)
        {
            return JToken.Parse(JsonConvert.SerializeObject(companyDetailPages.First(companyPage => (!string.IsNullOrWhiteSpace(companyPage.CompanyID) && companyPage.CompanyID.Equals(item["ID"].Value<string>())))._Url));
        }

    }
}