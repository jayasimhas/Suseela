using Glass.Mapper.Sc.Fields;
using Informa.Library.Services.Global;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.ViewModels.Casualty
{
    public class MarketDataTableWithDatasourceViewModel : GlassViewModel<IMarket_Data_Component_Parameters>
    {
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IGlobalSitecoreService GlobalService;

        public MarketDataTableWithDatasourceViewModel(IAuthenticatedUserContext authenticatedUserContext, 
            IGlobalSitecoreService globalService,
            IRenderingContextService renderingParametersService)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            GlobalService = globalService;
            RenderingParameters = renderingParametersService.GetCurrentRenderingParameters<IMarket_Data_Table_Types>();
        }
        public IMarket_Data_Table_Types RenderingParameters { get; set; }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        /// <summary>
        /// Page Title
        /// </summary>        
        public string PageTitle => GlobalService.GetItem<IGeneral_Content_Page>(Sitecore.Context.Item.ID.ToString()).Title;
        /// <summary>
        /// Table Title
        /// </summary>
        public string TableTitle => GlassModel?.Table_Title;
        /// <summary>
        /// Table SubTitle
        /// </summary>
        public string TableSubTitle => GlassModel?.Table_SubTitle;
        /// <summary>
        /// Dropdowns feed URL
        /// </summary>
        public string DropdownsFeedUrl => GlassModel?.Dropdowns_Feed_URL;
        /// <summary>
        /// Table data feed URL
        /// </summary>
        public string TankerFixHiddenVal => GlassModel?.Table_Result_Feed_URL;
        /// <summary>
        /// Data Provider
        /// </summary>
        public string DataProvider => GlassModel?.DataProvider;
        /// <summary>
        /// Data Provider Logo
        /// </summary>
        public Image ProviderLogo => GlassModel?.ProviderLogo;
        /// <summary>
        /// Populate Area dropdown
        /// </summary>
        public List<SelectListItem> PopulateByAreaDropdown => PopulateDropdown();

        /// <summary>
        /// Method to populate dropdowns
        /// </summary>
        /// <param name="dropdownType"></param>
        /// <returns></returns>
        public List<SelectListItem> PopulateDropdown()
        {
            var jsonAreaDropdown = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Views/Casualty/MarketDate.json"));
            List<SelectListItem> selectItemList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(jsonAreaDropdown))
            {
                var res = JsonConvert.DeserializeObject<JToken>(jsonAreaDropdown);
                foreach (JObject obj in res[0]["SelectDate"].Children())
                {
                    SelectListItem select = new SelectListItem { Text = obj["Text"].ToString(), Value = obj["Value"].ToString() };
                    selectItemList.Add(select);
                }
                return selectItemList;
            }

            else
            {
                return selectItemList;
            }
            //}
        }
    }
}
