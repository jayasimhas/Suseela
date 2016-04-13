using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Library.User.Document;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;

namespace Informa.Web.ViewModels
{
	public class FeaturedArticleViewModel : GlassArticleModel
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;

		public FeaturedArticleViewModel(
			IRenderingParametersContext renderingParametersContext,
			ISiteRootContext siterootContext,
			IArticleListItemModelFactory articleListableFactory, ITextTranslator textTranslator, IArticleSearch searcher,
            ISitecoreContext context,
            IArticleComponentFactory articleComponentFactory,
            IEntitledProductEntitlementAccessLevelContext entitledProductContext,
            IAuthenticatedUserContext authenticatedUserContext,
			IIsSavedDocumentContext isSavedDocumentContext,
            ICallToActionViewModel callToActionViewModel)
			: base(siterootContext, articleListableFactory, textTranslator, searcher, context, articleComponentFactory, entitledProductContext, authenticatedUserContext, isSavedDocumentContext, callToActionViewModel)
		{
			RenderingParametersContext = renderingParametersContext;
		}

		public override bool DisplayImage => Options.Show_Image ? base.DisplayImage : false;

		public IFeatured_Article_Options Options => RenderingParametersContext.GetParameters<IFeatured_Article_Options>();
	}
}