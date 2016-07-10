using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Services.Article;
using Informa.Library.User.Entitlement;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Utilities.Extensions;

namespace Informa.Web.ViewModels
{
	public class ArticleBodyContentModel : ArticleEntitledViewModel
	{
		public readonly ICallToActionViewModel CallToActionViewModel;
		protected readonly ITextTranslator TextTranslator;
		protected readonly IArticleService ArticleService;

		private readonly Lazy<string> _lazyBody;

		public ArticleBodyContentModel(
						IArticle model,
						IIsEntitledProducItemContext entitledProductContext,
						ITextTranslator textTranslator,
						ICallToActionViewModel callToActionViewModel,
						IArticleService articleService)
						: base(entitledProductContext)
		{
			TextTranslator = textTranslator;
			CallToActionViewModel = callToActionViewModel;
			ArticleService = articleService;

			_lazyBody = new Lazy<string>(() => IsFree || IsEntitled() ? ArticleService.GetArticleBody(model) : "");
		}

		public string Title => GlassModel.Title;
		public string Sub_Title => GlassModel.Sub_Title;
		public bool DisplayLegacyPublication => LegacyPublicationNames.Any();

		public IEnumerable<string> LegacyPublicationNames => ArticleService.GetLegacyPublicationNames(GlassModel);

		public string LegacyPublicationText => ArticleService.GetLegacyPublicationText(GlassModel);

		private string _summary;
		public string Summary => _summary ?? (_summary = ArticleService.GetArticleSummary(GlassModel));

		private IEnumerable<IPersonModel> _authors;
		public IEnumerable<IPersonModel> Authors
				=> _authors ?? (_authors = GlassModel.Authors.Select(x => new PersonModel(x)));

		private DateTime? _date;
		public DateTime Date
		{
			get
			{
				if (!_date.HasValue)
				{
					_date = GlassModel.GetDate();
				}
				return _date.Value;
			}
		}
		public string Category => GlassModel.Article_Category;
		public string Body => _lazyBody.Value;
		public string Content_Type => GlassModel.Content_Type?.Item_Name;
		public string Media_Type => ArticleService.GetMediaTypeName(GlassModel);
		public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);
		public string FeaturedImageSource => TextTranslator.Translate("Article.FeaturedImageSource");
	}
}