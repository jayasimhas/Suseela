using Informa.Library.Globalization;
using Informa.Library.Services.ExternalFeeds;
using Informa.Library.Services.Global;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.ViewModels.Casualty
{
    public class ShippingMovementsViewModel : GlassViewModel<IShipping_Result_Component_Parameters>
    {
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ICompaniesResultService CompanyResultService;
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public readonly ICallToActionViewModel CallToActionViewModel;
        public ShippingMovementsViewModel(IGlobalSitecoreService globalService,
          ITextTranslator textTranslator,
          ICompaniesResultService companyResultService,
          IAuthenticatedUserContext authenticatedUserContext,
          ICallToActionViewModel callToActionViewModel)
        {
            GlobalService = globalService;
            TextTranslator = textTranslator;
            CompanyResultService = companyResultService;
            AuthenticatedUserContext = authenticatedUserContext;
            CallToActionViewModel = callToActionViewModel;
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
        /// Filter by area dropdown Title
        /// </summary>
        public string FilterByAreaDropdownTitle => TextTranslator.Translate("Shipping.Movements.Area.Dropdown.Title");
        /// <summary>
        /// Filter by Movements Types dropdown title
        /// </summary>
        public string FilterByMovemetTypesDropdownTitle => TextTranslator.Translate("Shpping.Movements.Types.Dropdown.Title");
        /// <summary>
        /// Search Button Text
        /// </summary>
        public string SearchButtonText => TextTranslator.Translate("Shipping.Movements.Search.Button.Text");
        /// <summary>
        /// Populate Area dropdown
        /// </summary>
        public List<SelectListItem> PopulateByAreaDropdown => PopulateDropdown("AreaDropdown");
        /// <summary>
        /// Populate MovementTypes dropdown
        /// </summary>
        public List<SelectListItem> PopulateByMovementTypesDropdown => PopulateDropdown("MovementTypesDropdown");
       
        /// <summary>
        /// Method to populate dropdowns
        /// </summary>
        /// <param name="dropdownType"></param>
        /// <returns></returns>
        public List<SelectListItem> PopulateDropdown(string dropdownType)
        {
            if (!string.IsNullOrEmpty(ResultTableFeedUrl))
            {
                List<SelectListItem> selectItemList = new List<SelectListItem>();
                var jsonAreaDropdown = CompanyResultService.GetCompanyFeeds(ResultTableFeedUrl).Result;

                if (!string.IsNullOrEmpty(jsonAreaDropdown) && string.Equals(dropdownType, "AreaDropdown", StringComparison.OrdinalIgnoreCase))
                {
                    var res = JsonConvert.DeserializeObject<JToken>(jsonAreaDropdown);
                    foreach (JObject obj in res[0]["Areas"].Children())
                    {
                        SelectListItem select = new SelectListItem { Text = obj["Text"].ToString(), Value = obj["Value"].ToString() };
                        selectItemList.Add(select);
                    }
                    return selectItemList;
                }
                else if (string.Equals(dropdownType, "MovementTypesDropdown", StringComparison.OrdinalIgnoreCase))
                {
                    var res = JsonConvert.DeserializeObject<JToken>(jsonAreaDropdown);
                    foreach (JObject obj in res[0]["MovementTypes"].Children())
                    {
                        SelectListItem select = new SelectListItem { Text = obj["Text"].ToString(), Value = obj["Value"].ToString() };
                        selectItemList.Add(select);
                    }
                    return selectItemList;
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
            else
            {
                var jsonAreaDropdown = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Views/Casualty/dropdownValues.json"));
                List<SelectListItem> selectItemList = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(jsonAreaDropdown) && string.Equals(dropdownType, "AreaDropdown", StringComparison.OrdinalIgnoreCase))
                {
                    var res = JsonConvert.DeserializeObject<JToken>(jsonAreaDropdown);
                    foreach (JObject obj in res[0]["Areas"].Children())
                    {
                        SelectListItem select = new SelectListItem { Text = obj["Text"].ToString(), Value = obj["Value"].ToString() };
                        selectItemList.Add(select);
                    }
                    return selectItemList;
                }
                else if (string.Equals(dropdownType, "MovementTypesDropdown", StringComparison.OrdinalIgnoreCase))
                {
                    var res = JsonConvert.DeserializeObject<JToken>(jsonAreaDropdown);
                    foreach (JObject obj in res[0]["MovementTypes"].Children())
                    {
                        SelectListItem select = new SelectListItem { Text = obj["Text"].ToString(), Value = obj["Value"].ToString() };
                        selectItemList.Add(select);
                    }
                    return selectItemList;
                }
                else
                {
                    return new List<SelectListItem>();
                }
            }
        }
    }
}