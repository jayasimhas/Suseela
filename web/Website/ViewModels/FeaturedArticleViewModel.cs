using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Page;
using Informa.Library.Presentation;
using Informa.Library.Services.Article;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.StringUtils;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
	public class FeaturedArticleViewModel : GlassViewModel<IArticle>, IArticleBookmarker
	{
		protected readonly IArticleService ArticleService;
		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly IIsSavedDocumentContext IsSavedDocumentContext;
		protected readonly IPageItemContext PageItemContext;
		protected readonly IRenderingParametersContext RenderingParametersContext;
		protected readonly ITextTranslator TextTranslator;
		private DateTime? _date;
		private string _summary;

		public FeaturedArticleViewModel(
			IArticle model,
			IRenderingParametersContext renderingParametersContext,
			IArticleService articleService,
			IBylineMaker bylineMaker,
			IAuthenticatedUserContext authenticatedUserContext,
			IIsSavedDocumentContext isSavedDocumentContext,
			ITextTranslator textTranslator,
			IPageItemContext pageItemContext)
		{
			RenderingParametersContext = renderingParametersContext;
			ArticleService = articleService;
			AuthenticatedUserContext = authenticatedUserContext;
			IsSavedDocumentContext = isSavedDocumentContext;
			TextTranslator = textTranslator;
			PageItemContext = pageItemContext;

			ArticleByLine = bylineMaker.MakeByline(model.Authors);
			IsUserAuthenticated = AuthenticatedUserContext.IsAuthenticated;
			IsArticleBookmarked = IsSavedDocumentContext.IsSaved(model._Id);
			BookmarkText = TextTranslator.Translate("Bookmark");
			BookmarkedText = TextTranslator.Translate("Bookmarked");
			BookmarkPublication = articleService.GetArticlePublicationName(model);
			BookmarkTitle = model.ListableTitle;
		}

		public string Title => GlassModel.Title;
		public string Summary => _summary ?? (_summary = ArticleService.GetArticleSummary(GlassModel));
		public string Url => GlassModel._Url;
		public IEnumerable<ILinkable> ListableTopics => ArticleService.GetLinkableTaxonomies(GlassModel).Take(3);
		public bool DisplayImage => Options.Show_Image && !string.IsNullOrEmpty(Image?.ImageUrl);
		public IFeatured_Article_Options Options => RenderingParametersContext.GetParameters<IFeatured_Article_Options>();
		public string ArticleByLine { get; set; }

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
		public string BookmarkPublication { get; set; }
		public string BookmarkTitle { get; set; }
		public string PageTitle => PageItemContext.Get<I___BasePage>()?.Title;
		public bool IsUserAuthenticated { get; set; }
		public bool IsArticleBookmarked { get; set; }
		public string BookmarkText { get; set; }
		public string BookmarkedText { get; set; }
		public Guid ID => GlassModel._Id;
	}
}