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
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;

namespace Informa.Web.ViewModels.Casualty
{
    public class CasualtyReportViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ICompaniesResultService CompanyResultService;
       
        public CasualtyReportViewModel(IGlobalSitecoreService globalService,
            ISiteRootContext siterootContext,
            ITextTranslator textTranslator,
            ICompaniesResultService companyResultService,
            IRenderingContextService renderingParametersService)
        {
            GlobalService = globalService;
            SiterootContext = siterootContext;
            TextTranslator = textTranslator;
            CompanyResultService = companyResultService;
            feedUrl = renderingParametersService.GetCurrentRendering().Parameters["feedurl"];
        }
        public string feedUrl { get; set; }
        public string Title => GlassModel?.Title;
        public string FilterByDateDropdownTitle => TextTranslator.Translate("Casualty.Report.ByDate.DropdownTitle");
        public string FilterBySectionDropdownTitle => TextTranslator.Translate("Casualty.Report.BySection.DropdownTitle");
        public string CasualtyDetailPageUrl => GetCasualtyDetailPageUrl();
        public string jsonCasualtyData => GetCasualtyData(feedUrl);
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

        public SelectList FilterBySectionDropdownValues
        {
            get
            {
                List<SelectListItem> datesList = new List<SelectListItem>();
                //for (int i = 1; i <= 7; i++)
                //{
                SelectListItem section1 = new SelectListItem { Text = "MARINE", Value = "#marine" };
                SelectListItem section2 = new SelectListItem { Text = "SEIZURES & ARRESTS", Value = "#seizures&arrests" };
                SelectListItem section3 = new SelectListItem { Text = "PIPELINE INCIDENTS", Value = "#pipelineincidents" };
                datesList.Add(section1);
                datesList.Add(section2);
                datesList.Add(section3);
                //}
                return new SelectList(datesList, "Value", "Text");
            }
        }

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