using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels.DataTools
{
    public class AMChartViewModel: GlassViewModel<IAMcharts_Dashboard>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITextTranslator TextTranslator;
        public AMChartViewModel(ISiteRootContext siteRootContext, IGlobalSitecoreService globalService, ITextTranslator textTranslator)
        {
            SiteRootContext = siteRootContext;
            GlobalService = globalService;
            TextTranslator = textTranslator;
        }

        #region AMcharts dashboard parameters
       
        public string ChartType => GlassModel?.ChartType;
        public string CategoryField => GlassModel?.CategoryField;
        public string ValueField => GlassModel?.ValueField;
        public string Height => GlassModel?.Height;
        public string Width => GlassModel?.Width;
        public string JsonPath => GlassModel?.JsonPath?.Url;

        #endregion

        #region AMcharts landing page component content        
        public string PageTitle => GlassModel?.Page_Title;
        public string PageSubheading => GlassModel?.Page_Subheading;
        public string IntroductoryText => GlassModel?.Introductory_Text;
        public string ToolExplanation => GlassModel?.Tool_Explanation;
        public string ShowDemoLable => TextTranslator.Translate("DataTools.ShowDemo");
        public string HideDemoLable => TextTranslator.Translate("DataTools.HideDemo");
        #endregion

        #region AMcharts Right rail component content  
        public string LandingPageLink => GlassModel?.Landing_Page_Link?.Url;
        public string LandingPageLinkLable => TextTranslator.Translate("DataTools.LandingPageLink");
        #endregion
    }
}