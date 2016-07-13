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
	public class RelatedCompaniesModel : GlassViewModel<IArticle>
	{
		[AutowireService(true)]
		public interface IDependencies
		{
			ITextTranslator TextTranslator { get; set; }
			IRelatedCompaniesService RelatedCompaniesService { get; set; }
		}

		public RelatedCompaniesModel(IArticle article, IDependencies dependencies)
		{
			ComponentTitle = dependencies.TextTranslator.Translate(DictionaryKeys.ArticleRelatedCompaniesTitle);
			RelatedCompanies = dependencies.RelatedCompaniesService.GetRelatedCompanyLinks(article);
			ArticleName = article.Title;
		}

		public string ComponentTitle { get; set; }
		public IEnumerable<Link> RelatedCompanies { get; set; } 

		// Click through source
		public string ArticleName { get; set; }
	}
}