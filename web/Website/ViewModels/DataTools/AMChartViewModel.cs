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
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels.DataTools
{
    public class AMChartViewModel: GlassViewModel<IAMcharts_Dashboard>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IArticleSearch Searcher;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        private readonly IAuthenticatedUserContext _authenticatedUserContext;
        public readonly ICallToActionViewModel CallToActionViewModel;
        public AMChartViewModel(ISiteRootContext siteRootContext, 
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
       
        public string ChartType => GlassModel?.ChartType;
        public string CategoryField => GlassModel?.CategoryField;
        public string ValueField => GlassModel?.ValueField;
        public string Height => GlassModel?.Height;
        public string Width => GlassModel?.Width;
        public string JsonPath => GlassModel?.JsonPath?.Url;

        #endregion

        #region AMcharts landing page component content   
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
        public string LandingPageLinkLable => TextTranslator.Translate("DataTools.LandingPageLink");
        public string RealatedContentLableText => TextTranslator.Translate("DataTools.RelatedContentLable");
        public IEnumerable<IListable> RelatedArticles => GlassModel?.Related_Articles.
            Where(r => r != null).Select(x => ArticleListableFactory.Create(GlobalService.GetItem<IArticle>(x._Id))).
            Cast<IListable>().OrderByDescending(x => x.ListableDate);
        #endregion


    }
}