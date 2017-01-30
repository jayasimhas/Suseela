using Glass.Mapper.Sc.Fields;
using Informa.Library.Services.Global;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.ViewModels.Casualty
{
    public class MarketDataViewModel : GlassViewModel<IMarket_Data_Component_Parameters>
    {
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IGlobalSitecoreService GlobalService;

        public MarketDataViewModel(IAuthenticatedUserContext authenticatedUserContext, IGlobalSitecoreService globalService)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            GlobalService = globalService;
        }
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
        public string ResultTableFeedUrl => GlassModel?.Table_Result_Feed_URL;
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
            //if (!string.IsNullOrEmpty(DropdownsFeedUrl))
            //{
            //    List<SelectListItem> selectItemList = new List<SelectListItem>();
            //    var jsonAreaDropdown = CompanyResultService.GetCompanyFeeds(DropdownsFeedUrl).Result;

            //    if (!string.IsNullOrEmpty(jsonAreaDropdown) && string.Equals(dropdownType, "AreaDropdown", StringComparison.OrdinalIgnoreCase))
            //    {
            //        var res = JsonConvert.DeserializeObject<JToken>(jsonAreaDropdown);
            //        foreach (JObject obj in res[0]["Areas"].Children())
            //        {
            //            SelectListItem select = new SelectListItem { Text = obj["Text"].ToString(), Value = obj["Value"].ToString() };
            //            selectItemList.Add(select);
            //        }
            //        return selectItemList;
            //    }

            //    else
            //    {
            //        return new List<SelectListItem>();
            //    }
            //}
            //else
            //{
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
