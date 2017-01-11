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

namespace Informa.Web.ViewModels.Casualty
{
    public class CasualtyReportViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ICompaniesResultService CompanyResultService;
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public readonly ICallToActionViewModel CallToActionViewModel;

        public CasualtyReportViewModel(IGlobalSitecoreService globalService,
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
            feedUrl = renderingParametersService.GetCurrentRendering().Parameters["feedurl"];
        }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        /// <summary>
        /// feedURL
        /// </summary>
        public string feedUrl { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title => GlassModel?.Title;
        /// <summary>
        /// Filter by date dropdown title
        /// </summary>
        public string FilterByDateDropdownTitle => TextTranslator.Translate("Casualty.Report.ByDate.DropdownTitle");
        /// <summary>
        /// Filter by section dropdown title
        /// </summary>
        public string FilterBySectionDropdownTitle => TextTranslator.Translate("Casualty.Report.BySection.DropdownTitle");
        /// <summary>
        /// Casualty detail page URL
        /// </summary>
        public string CasualtyDetailPageUrl => GetCasualtyDetailPageUrl();
        /// <summary>
        /// Json Data
        /// </summary>
        public string jsonCasualtyData => GetCasualtyData(feedUrl);
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
                    if(casualtyDetailPage!=null)
                    {
                        return casualtyDetailPage._AbsoluteUrl;
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Filter by date dropdown values
        /// </summary>
        public SelectList FilterByDateDropdownValues
        {
            get
            {
                List<SelectListItem> datesList = new List<SelectListItem>();
                for (int i = 1; i <= 7; i++)
                {
                    SelectListItem date = new SelectListItem { Text = DateTime.Today.AddDays(-i).ToString("ddd, dd MMMM yyyy"), Value = DateTime.Today.AddDays(-i).ToString("dd-MM-yyyy") };
                    datesList.Add(date);
                }
                return new SelectList(datesList, "Value", "Text");
            }
        }
        /// <summary>
        /// Method to get Json Data from external URL
        /// </summary>
        /// <param name="feedURL">feed URL</param>
        /// <returns>JSON Data</returns>
        public string GetCasualtyData(string feedURL)
        {
            if (!string.IsNullOrEmpty(feedURL))
            {
                return CompanyResultService.GetCompanyFeeds(feedURL).Result;
            }
            else
            {
                return string.Empty;
            }
            
        }
    }
}