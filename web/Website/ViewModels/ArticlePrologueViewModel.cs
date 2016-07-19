using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class ArticlePrologueViewModel : GlassViewModel<IGlassBase>
	{
		public ArticlePrologueViewModel(
			IArticleProloguePrintViewModel articleProloguePrintViewModel,
			IArticlePrologueEmailViewModel articlePrologueEmailViewModel,
			IArticlePrologueBookmarkViewModel articlePrologueBookmarkViewModel,
			IArticlePrologueShareViewModel articlePrologueShareViewModel,
			IArticleTagsViewModel articleTagsViewModel,
            IArticlePrologueAskTheAnalystViewModel articlePrologueAskTheAnalystViewModel)
		{
			PrintViewModel = articleProloguePrintViewModel;
			EmailViewModel = articlePrologueEmailViewModel;
			BookmarkViewModel = articlePrologueBookmarkViewModel;
			ShareViewModel = articlePrologueShareViewModel;
			ArticleTagsViewModel = articleTagsViewModel;
            AskTheAnalystViewModel = articlePrologueAskTheAnalystViewModel;
        }

		public IArticleProloguePrintViewModel PrintViewModel;

		public IArticlePrologueEmailViewModel EmailViewModel;

		public IArticlePrologueBookmarkViewModel BookmarkViewModel;

		public IArticlePrologueShareViewModel ShareViewModel;

		public IArticleTagsViewModel ArticleTagsViewModel;

        public IArticlePrologueAskTheAnalystViewModel AskTheAnalystViewModel;
    }
}