using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.PerScope)]
	public class ArticlePrologueShareViewModel : IArticlePrologueShareViewModel
	{
        protected readonly ITextTranslator TextTranslator;
        protected readonly IRenderingItemContext ArticleRenderingContext;

        public ArticlePrologueShareViewModel(
            ITextTranslator textTranslator,
            IRenderingItemContext articleRenderingContext)
        {
            TextTranslator = textTranslator;
            ArticleRenderingContext = articleRenderingContext;
        }

        public string ArticleTitle => ArticleRenderingContext.Get<IArticle>().Title;
        public string ArticleUrl => $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}{ArticleRenderingContext.Get<IArticle>()._Url}";
        public string ShareText => TextTranslator.Translate("Article.Share");
    }
}