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
        public string ResultsData => !string.IsNullOrWhiteSpace(GlassModel.ExternalFeedUrl) ? 
                                     companyResultService.GetCompanyFeeds(GlassModel.ExternalFeedUrl).Result :
                                     "External feed url is empty";

        public string JsonMappingData => GetJsonMappingData();

        private string GetJsonMappingData()
        {
            string result = string.Empty;
            var graphTypeNode = sitecoreContext.GetItem<Item>(Settings.GetSetting("Company.Graph.Type"));
            List<ICompany_Graph> companyGraphItems = new List<ICompany_Graph>();

            if(graphTypeNode != null && graphTypeNode.Children != null && graphTypeNode.Children.Any())
            {
                companyGraphItems.AddRange(graphTypeNode.Children.Select(item => sitecoreContext.GetItem<ICompany_Graph>(item.ID.Guid)));

                if(companyGraphItems != null && companyGraphItems.Any())
                    result = JsonConvert.SerializeObject(companyGraphItems.Select(x => new { Key = x.GraphID, Value = x.GraphTitle } ));
            }

            return result;
        }
       
    }
}