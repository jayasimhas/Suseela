using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Article.Companies;
using Informa.Library.Globalization;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
	public class RelatedDealsModel : GlassViewModel<IArticle>
	{
		[AutowireService(true)]
		public interface IDependencies
		{
			ITextTranslator TextTranslator { get; set; }
			IRelatedDealsService RelatedDealsService { get; set; }
		}

		public RelatedDealsModel(IArticle article, IDependencies dependencies)
		{
			ComponentTitle = dependencies.TextTranslator.Translate(DictionaryKeys.ArticleRelatedDealsTitle);
			RelatedDeals = dependencies.RelatedDealsService.GetRelatedDeals(article);
			ArticleName = article.Title;
		}

		public string ComponentTitle { get; set; }
		public IEnumerable<Link> RelatedDeals { get; set; }
		public string ArticleName { get; set; }
	}
}