using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.Articles
{
	public class ArticlePrologueViewModel : GlassViewModel<IGlassBase>
	{
        protected readonly ITextTranslator TextTranslator;
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly ISiteRootContext SiteRootContext;

        public ArticlePrologueViewModel(
			IArticleProloguePrintViewModel articleProloguePrintViewModel,
			IArticlePrologueEmailViewModel articlePrologueEmailViewModel,
			IArticlePrologueBookmarkViewModel articlePrologueBookmarkViewModel,
			IArticlePrologueShareViewModel articlePrologueShareViewModel,
			IArticleTagsViewModel articleTagsViewModel,
            IArticlePrologueATAViewModel articleATAViewModel,
             ISiteRootContext siteRootContext,
               ITextTranslator textTranslator,
               IAuthenticatedUserContext userContext)
		{
			PrintViewModel = articleProloguePrintViewModel;
			EmailViewModel = articlePrologueEmailViewModel;
			BookmarkViewModel = articlePrologueBookmarkViewModel;
			ShareViewModel = articlePrologueShareViewModel;
			ArticleTagsViewModel = articleTagsViewModel;
            ATAViewModel = articleATAViewModel;
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            UserContext = userContext;
        }

		public IArticleProloguePrintViewModel PrintViewModel;

        public IArticlePrologueATAViewModel ATAViewModel;

		public IArticlePrologueEmailViewModel EmailViewModel;

		public IArticlePrologueBookmarkViewModel BookmarkViewModel;

		public IArticlePrologueShareViewModel ShareViewModel;

		public IArticleTagsViewModel ArticleTagsViewModel;

        public bool IsActiveAskAnalyst => SiteRootContext.Item.Is_Active_Ask_The_Analyst;
    }
}