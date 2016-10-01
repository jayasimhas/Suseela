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
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.Page;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.ViewModels.Articles;

namespace Informa.Web.ViewModels
{
	public class FeaturedArticleViewModel : GlassViewModel<IArticle>, IArticleBookmarker
	{
	    private readonly IDependencies _dependencies;

	    [AutowireService(true)]
	    public interface IDependencies
	    {
            IRenderingParametersContext RenderingParametersContext { get; }
            IArticleService ArticleService { get; }
            IAuthenticatedUserContext AuthenticatedUserContext { get; }
            IIsSavedDocumentContext IsSavedDocumentContext { get; }
            ITextTranslator TextTranslator { get; }
            IBylineMaker BylineMaker { get; }
            IPageItemContext PageItemContext { get; }
	    }

		public FeaturedArticleViewModel(IDependencies dependencies)
		{
		    _dependencies = dependencies;

		    BookmarkText = _dependencies.TextTranslator.Translate("Bookmark");
            BookmarkedText = _dependencies.TextTranslator.Translate("Bookmarked");
		    IsUserAuthenticated = _dependencies.AuthenticatedUserContext.IsAuthenticated;
			BookmarkPublication = _dependencies.ArticleService.GetArticlePublicationName(GlassModel);
			//BookmarkTitle = GlassModel.ListableTitle;
		}

		public string Title => GlassModel.Title;
		private string _summary;
		public string Summary => _summary ?? (_summary = _dependencies.ArticleService.GetArticleSummary(GlassModel));
		public string Url => GlassModel._Url;
		public IEnumerable<ILinkable> ListableTopics => _dependencies.ArticleService.GetLinkableTaxonomies(GlassModel).Take(3);
		public bool DisplayImage => Options.Show_Image && !string.IsNullOrEmpty(Image?.ImageUrl);
		public IFeatured_Article_Options Options => _dependencies.RenderingParametersContext.GetParameters<IFeatured_Article_Options>();

	    private string _articleByLine;
	    public string ArticleByLine
	        => _articleByLine ?? (_articleByLine = _dependencies.BylineMaker.MakeByline(GlassModel.Authors));

		private DateTime? _date;
	    public DateTime Date => _date ?? (_date = GlassModel.GetDate()).Value;
		
		public string ContentType => GlassModel.Content_Type?.Item_Name;
		public string MediaType => _dependencies.ArticleService.GetMediaTypeIconData(GlassModel)?.MediaType;
		public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);
	    public bool IsUserAuthenticated { get; set; }
	    public string BookmarkText { get; set; }
        public string BookmarkedText { get; set; }
	    private bool? _isArticleBookmarked;
        public bool IsArticleBookmarked {
            get
            {
                return _isArticleBookmarked ??
                       (_isArticleBookmarked = _dependencies.IsSavedDocumentContext.IsSaved(GlassModel._Id)).Value;
            }
            set { _isArticleBookmarked = value; } }


        public Guid ID => GlassModel._Id;
		public string BookmarkPublication { get; set; }
		public string BookmarkTitle { get; set; }
		public string PageTitle => _dependencies.PageItemContext.Get<I___BasePage>()?.Title;
	}
}