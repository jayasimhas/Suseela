﻿using Informa.Library.Article.Search;
using Informa.Library.DataTools;
using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Web.ViewModels.Articles;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Collections.Generic;
using System.Linq;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.User.Authentication;
using Sitecore.Data.Fields;

namespace Informa.Web.ViewModels.DataTools
{
    public class TableauDashboardViewModel : GlassViewModel<ITableau_Dashboard>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITableauUtil TableauUtil;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IArticleSearch Searcher;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        private readonly IAuthenticatedUserContext _authenticatedUserContext;
        public readonly ICallToActionViewModel CallToActionViewModel;

        public TableauDashboardViewModel(
            ISiteRootContext siteRootContext,
            IGlobalSitecoreService globalService,
            ITableauUtil tableauUtil,
            ITextTranslator textTranslator,
            IDataToolPrologueViewModel dataToolPrologueViewModel,
            IArticleListItemModelFactory articleListableFactory,
            IArticleSearch searcher,
            ICallToActionViewModel callToActionViewModel,
             IAuthenticatedUserContext authenticatedUserContext)
        {
            SiteRootContext = siteRootContext;
            GlobalService = globalService;
            TableauUtil = tableauUtil;
            TextTranslator = textTranslator;
            PrologueViewModel = dataToolPrologueViewModel;
            ArticleListableFactory = articleListableFactory;
            Searcher = searcher;
            CallToActionViewModel = callToActionViewModel;
            _authenticatedUserContext = authenticatedUserContext;
        }

        #region Tableau Dashboard Parameters/details
        
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
        public string ToolExplanation => GlassModel?.Tool_Explanation;

        public string ShowDemoLable => TextTranslator.Translate("DataTools.ShowDemo");

        public string HideDemoLable => TextTranslator.Translate("DataTools.HideDemo");

        public IDataToolPrologueViewModel PrologueViewModel;

        public IEnumerable<IListable> RelatedArticles => GlassModel?.Related_Articles.
            Where(r => r != null).Select(x => ArticleListableFactory.Create(GlobalService.GetItem<IArticle>(x._Id))).
            Cast<IListable>().OrderByDescending(x => x.ListableDate);

        public string RealatedContentLableText => TextTranslator.Translate("DataTools.RelatedContentLable");

        public bool IsUserAuthenticated => _authenticatedUserContext.IsAuthenticated;

        #endregion

        #region Tableau right rail component content
        
        public string LandingPageLink => GlassModel?.Landing_Page_Link?.Url;

        public string LandingPageLinkLable => TextTranslator.Translate("DataTools.LandingPageLink");

        #endregion

        #region Tableau server details

        public string HostUrl => GlobalService.GetItemByTemplateId(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.Server_NameFieldId];
        
        public LinkField JSAPILinkField => GlobalService.GetItemByTemplateId(ITableau_ConfigurationConstants.TemplateId.ToString()).Fields[ITableau_ConfigurationConstants.JS_API_UrlFieldId];
        
        public string JSAPIUrl => JSAPILinkField.Url;

        public string TableauTicket => TableauUtil.GenerateSecureTicket(GlobalService.GetItemByTemplateId(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.Server_NameFieldId],
            GlobalService.GetItemByTemplateId(ITableau_ConfigurationConstants.TemplateId.ToString())[ITableau_ConfigurationConstants.User_NameFieldId]);
        #endregion

    }
}