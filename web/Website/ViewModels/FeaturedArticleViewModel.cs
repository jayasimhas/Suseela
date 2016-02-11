using Informa.Library.Presentation;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;

namespace Informa.Web.ViewModels
{
	public class FeaturedArticleViewModel : GlassArticleModel
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;

		public FeaturedArticleViewModel(
			IRenderingParametersContext renderingParametersContext,
			ISiteRootContext siterootContext,
			IArticleListItemModelFactory articleListableFactory)
			: base(siterootContext, articleListableFactory)
		{
			RenderingParametersContext = renderingParametersContext;
		}

		public override bool DisplayImage => Options.Show_Image ? base.DisplayImage : false;

		public IFeatured_Article_Options Options => RenderingParametersContext.GetParameters<IFeatured_Article_Options>();
	}
}