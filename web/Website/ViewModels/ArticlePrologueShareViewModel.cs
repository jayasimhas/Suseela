using System;
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
		private readonly Lazy<IArticle> _article; 

		protected readonly ITextTranslator TextTranslator;
		
		public ArticlePrologueShareViewModel(
				ITextTranslator textTranslator,
				IRenderingItemContext articleRenderingContext)
		{
			TextTranslator = textTranslator;
			
			_article = new Lazy<IArticle>(articleRenderingContext.Get<IArticle>);
		}

		public string ArticleTitle => _article.Value.Title;
		public string ArticleUrl => $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}{_article.Value._Url}";
		public string ShareText => TextTranslator.Translate("Article.Share");
	}
}