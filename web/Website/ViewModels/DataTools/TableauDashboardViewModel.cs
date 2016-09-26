using Informa.Library.DataTools;
using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
    public class TableauDashboardViewModel : GlassViewModel<ITableau_Dashboard>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITableauUtil TableauUtil;
        protected readonly ITextTranslator TextTranslator;

        public TableauDashboardViewModel(
            ISiteRootContext siteRootContext, 
            IGlobalSitecoreService globalService,
            ITableauUtil tableauUtil, 
            ITextTranslator textTranslator)
        {
            SiteRootContext = siteRootContext;
            GlobalService = globalService;
            TableauUtil = tableauUtil;
            TextTranslator = textTranslator;
        }

        #region Tableau Dashboard Parameters/details

        public bool IsRightRail => GlassModel.Is_Right_Rail;

        public string DashboardName => GlassModel?.Dashboard_Name;

        public string MobileDashboardName => GlassModel?.Mobile_Dashboard_Name;

        public bool AuthenticationRequired => GlassModel.Authentication_Required;

        public bool DisplayTabs => GlassModel.Display_Tabs;

        public bool DisplayToolbars => GlassModel.Display_Toolbars;

        public bool AllowCustomViews => GlassModel.Allow_Custom_Views;

        public string Filter => GlassModel?.Filter;

        public string DashboardWidth => GlassModel?.Width;

        public string DashboardHeight => GlassModel?.Height;

        public bool IsMobileDashboardAvailable => !string.IsNullOrWhiteSpace(MobileDashboardName);

        #endregion

        #region Tableau landing page component content

        public string PageTitle => GlassModel?.Page_Title;

        public string PageSubheading => GlassModel?.Page_Subheading;

        public string IntroductoryText => GlassModel?.Introductory_Text;

        public string IntroductoryVideoLink => GlassModel?.Introductory_Video.Url;

        public string ToolExplanation => GlassModel?.Tool_Explanation;

        public string ShowDemoLable => TextTranslator.Translate("DataTools.ShowDemo");

        public string HideDemoLable => TextTranslator.Translate("DataTools.HideDemo");

        #endregion

        #region Tableau right rail component content

        public string ComponentHeading => GlassModel?.Heading;

        public string LandingPageLink => GlassModel?.Landing_Page_Link.Url;

        public string LandingPageLinkLable => TextTranslator.Translate("DataTools.LandingPageLink");

        #endregion

        #region Tableau server details

        public string HostUrl => GlobalService.GetItemByTemplateId(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.Server_NameFieldId];

        public string JSAPIUrl => GlobalService.GetItemByTemplateId(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.JS_API_UrlFieldId];

        public string TableauTicket => TableauUtil.GenerateSecureTicket(GlobalService.GetItemByTemplateId(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.Server_NameFieldId],
            GlobalService.GetItemByTemplateId(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.User_NameFieldId]);
        #endregion

    }
}