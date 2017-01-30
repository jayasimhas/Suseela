using Informa.Library.Globalization;
using Informa.Library.Services.ExternalFeeds;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Informa.Web.ViewModels.Casualty
{
    public class CasualtyListViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ICompaniesResultService CompanyResultService;
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public readonly ICallToActionViewModel CallToActionViewModel;

        public CasualtyListViewModel(IGlobalSitecoreService globalService,
            ISiteRootContext siterootContext,
            ITextTranslator textTranslator,
            ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService,
            IAuthenticatedUserContext authenticatedUserContext,
            ICallToActionViewModel callToActionViewModel)
        {
            GlobalService = globalService;
            SiterootContext = siterootContext;
            TextTranslator = textTranslator;
            CompanyResultService = companyResultService;
            AuthenticatedUserContext = authenticatedUserContext;
            CallToActionViewModel = callToActionViewModel;
            feedUrlConfigurationItem = renderingParametersService.GetCurrentRenderingParameters<IExternal_Feed_Url_Configuration>();
        }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        /// <summary>
        /// feed URL
        /// </summary>
        public IExternal_Feed_Url_Configuration feedUrlConfigurationItem { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title => GlassModel?.Title;
        /// <summary>
        /// Filter by date dropdown Title
        /// </summary>
        public string FilterByDateDropdownTitle => TextTranslator.Translate("Casualty.Report.ByDate.DropdownTitle");
        /// <summary>
        /// Filter by Section dropdown Title
        /// </summary>
        public string FilterBySectionDropdownTitle => TextTranslator.Translate("Casualty.Report.BySection.DropdownTitle");
        /// <summary>
        /// Casualty detail page URL
        /// </summary>
        public string CasualtyDetailPageUrl => GetCasualtyDetailPageUrl();
        /// <summary>
        /// Casualty data
        /// </summary>
        public string jsonCasualtyData => GetCasualtyData();
        /// <summary>
        /// Method to get Casualty detail page URL
        /// </summary>
        /// <returns></returns>
        private string GetCasualtyDetailPageUrl()
        {
            var currentItem = GlobalService.GetItem<IGeneral_Content_Page>(Sitecore.Context.Item.ID.ToString());

            if (currentItem != null)
            {
                var casualtyDetailPageRoot = currentItem._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();
                if (casualtyDetailPageRoot != null)
                {
                    var casualtyDetailPage = casualtyDetailPageRoot._ChildrenWithInferType.OfType<IGeneral_Content_Page>().FirstOrDefault();
                    if (casualtyDetailPage != null)
                    {
                        return casualtyDetailPage._AbsoluteUrl;
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Date filter dropdown values
        /// </summary>
        public SelectList FilterByDateDropdownValues
        {
            get
            {
                string jsonDateDropdown = string.Empty;
                if (feedUrlConfigurationItem != null && !string.IsNullOrEmpty(feedUrlConfigurationItem.External_Feed_URL))
                {
                    jsonDateDropdown = GetCasualtyData();
                }
                else
                {
                    jsonDateDropdown = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Views/Casualty/CasualtyListing.json"));
                }

                List<SelectListItem> selectItemList = new List<SelectListItem>();
                if (jsonDateDropdown != null && !string.IsNullOrEmpty(jsonDateDropdown))
                {
                    JArray array = JArray.Parse(jsonDateDropdown);
                    foreach (JObject content in array.Children<JObject>())
                    {
                        foreach (JProperty prop in content.Properties())
                        {
                            SelectListItem select = new SelectListItem { Text = DateTime.ParseExact(prop.Name, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("ddd, dd MMMM yyyy"), Value = prop.Name.ToString() };
                            selectItemList.Add(select);
                        }
                    }
                    return new SelectList(selectItemList, "Value", "Text");
                }
                else
                {
                    return new SelectList(selectItemList, "Value", "Text");
                }

            }
        }
        /// <summary>
        /// Method to get casualty data from external feed url
        /// </summary>
        /// <param name="feedURL"></param>
        /// <returns></returns>
        public string GetCasualtyData()
        {
            if (feedUrlConfigurationItem != null && !string.IsNullOrEmpty(feedUrlConfigurationItem.External_Feed_URL))
            {
                return CompanyResultService.GetCompanyFeeds(feedUrlConfigurationItem.External_Feed_URL).Result;
            }
            else
            {
                return string.Empty;
            }

        }
    }
}