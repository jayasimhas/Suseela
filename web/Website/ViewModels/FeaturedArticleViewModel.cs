using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.Services.Article;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.StringUtils;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
	public class FeaturedArticleViewModel : GlassViewModel<IArticle>, IArticleBookmarker
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;
		protected readonly IArticleService ArticleService;
	    protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
	    protected readonly IIsSavedDocumentContext IsSavedDocumentContext;
	    protected readonly ITextTranslator TextTranslator;
		
		public FeaturedArticleViewModel(
				IArticle model,
				IRenderingParametersContext renderingParametersContext,
				IArticleService articleService,
				IBylineMaker bylineMaker,
                IAuthenticatedUserContext authenticatedUserContext,
                IIsSavedDocumentContext isSavedDocumentContext,
                ITextTranslator textTranslator)
		{
			RenderingParametersContext = renderingParametersContext;
			ArticleService = articleService;
		    AuthenticatedUserContext = authenticatedUserContext;
		    IsSavedDocumentContext = isSavedDocumentContext;
		    TextTranslator = textTranslator;

            ArticleByLine = bylineMaker.MakeByline(model.Authors);
		    IsUserAuthenticated = AuthenticatedUserContext.IsAuthenticated;
		    IsArticleBookmarked = IsSavedDocumentContext.IsSaved(model._Id);
		    BookmarkText = TextTranslator.Translate("Bookmark");
            BookmarkedText = TextTranslator.Translate("Bookmarked");
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
        public bool IsUserAuthenticated { get; set; }
        public bool IsArticleBookmarked { get; set; }
        public string BookmarkText { get; set; }
        public string BookmarkedText { get; set; }

		public Guid ID => GlassModel._Id;
	}
}