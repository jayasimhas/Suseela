using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.ViewModels.Articles;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.DataTools
{
    public class AMChartsUsingBuilderViewModel: GlassViewModel<IAMCharts_Using_Builder>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IArticleSearch Searcher;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        private readonly IAuthenticatedUserContext _authenticatedUserContext;
        public readonly ICallToActionViewModel CallToActionViewModel;
        public AMChartsUsingBuilderViewModel(ISiteRootContext siteRootContext,
            IGlobalSitecoreService globalService,
            ITextTranslator textTranslator,
            IDataToolPrologueViewModel dataToolPrologueViewModel,
            IArticleListItemModelFactory articleListableFactory,
            IArticleSearch searcher,
            ICallToActionViewModel callToActionViewModel,
             IAuthenticatedUserContext authenticatedUserContext)
        {
            SiteRootContext = siteRootContext;
            GlobalService = globalService;
            TextTranslator = textTranslator;
            PrologueViewModel = dataToolPrologueViewModel;
            ArticleListableFactory = articleListableFactory;
            Searcher = searcher;
            CallToActionViewModel = callToActionViewModel;
            _authenticatedUserContext = authenticatedUserContext;
        }

        #region AMcharts dashboard parameters
        /// <summary>
        /// AMchart Input Parameter: Chart Type
        /// </summary>
        public string ChartType => GlassModel?.ChartType;
        /// <summary>
        /// AMchart Input Parameter: Chart Height
        /// </summary>
        public string ChartHeight => GlassModel?.ChartHeight;
        /// <summary>
        /// AMchart Input Parameter: JSON Data Path
        /// </summary>
        public string JsonDataPath => GlassModel?.JsonDataUrl?.Url;

        /// <summary>
        /// AMchart Input Parameter: Chart Presentation part
        /// </summary>
        public string Presentation => GlassModel?.Presentation;
        #endregion

        #region AMcharts landing page component content parameters 
        public bool IsUserAuthenticated => _authenticatedUserContext.IsAuthenticated;
        public bool AuthenticationRequired => GlassModel.Authentication_Required;
        public string PageTitle => GlassModel?.Page_Title;
        public string PageSubheading => GlassModel?.Page_Subheading;
        public string IntroductoryText => GlassModel?.Introductory_Text;
        public string ToolExplanation => GlassModel?.Tool_Explanation;
        public string ShowDemoLable => TextTranslator.Translate("DataTools.ShowDemo");
        public string HideDemoLable => TextTranslator.Translate("DataTools.HideDemo");
        #endregion
        public IDataToolPrologueViewModel PrologueViewModel;

        #region AMcharts Right rail component content
        public string LandingPageLink => GlassModel?.Landing_Page_Link?.Url;
        public string LandingPageLinkLable => string.IsNullOrWhiteSpace(GlassModel?.Landing_Page_Link?.Text) ? TextTranslator.Translate("DataTools.LandingPageLink") : GlassModel?.Landing_Page_Link?.Text; 
        public string RealatedContentLableText => TextTranslator.Translate("DataTools.RelatedContentLable");
        /// <summary>
        /// Related Articles for AMchart
        /// </summary>
        public IEnumerable<IListable> RelatedArticles => GlassModel?.Related_Articles.
            Where(r => r != null).Select(x => ArticleListableFactory.Create(GlobalService.GetItem<IArticle>(x._Id))).
            Cast<IListable>().OrderByDescending(x => x.ListableDate);
        #endregion

    }
}