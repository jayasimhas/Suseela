using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Presentation;
using Informa.Library.Services.Article;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.StringUtils;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
	public class FeaturedArticleViewModel : GlassViewModel<IArticle>
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;
		protected readonly IArticleService ArticleService;
		
		public FeaturedArticleViewModel(
				IArticle model,
				IRenderingParametersContext renderingParametersContext,
				IArticleService articleService,
				IBylineMaker bylineMaker)
		{
			RenderingParametersContext = renderingParametersContext;
			ArticleService = articleService;

			ArticleByLine = bylineMaker.MakeByline(model.Authors);
		}

		public string Title => GlassModel.Title;
		private string _summary;
		public string Summary => _summary ?? (_summary = ArticleService.GetArticleSummary(GlassModel));
		public string Url => GlassModel._Url;
		public IEnumerable<ILinkable> ListableTopics => ArticleService.GetLinkableTaxonomies(GlassModel).Take(3);
		public bool DisplayImage => Options.Show_Image && !string.IsNullOrEmpty(Image?.ImageUrl);
		public IFeatured_Article_Options Options => RenderingParametersContext.GetParameters<IFeatured_Article_Options>();
		public string ArticleByLine { get; set; }
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
		public string Content_Type => GlassModel.Content_Type?.Item_Name;
		public string Media_Type => ArticleService.GetMediaTypeName(GlassModel);
		public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);
	}
}