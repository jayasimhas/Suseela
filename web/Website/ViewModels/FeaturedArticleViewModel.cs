using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.Search.Utilities;
using Informa.Library.User.Entitlement;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;

namespace Informa.Web.ViewModels
{
	public class FeaturedArticleViewModel : ArticleBodyContentModel
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;

		public FeaturedArticleViewModel(
            IArticle model,
			IRenderingParametersContext renderingParametersContext, 
            ITextTranslator textTranslator,
			IIsEntitledProducItemContext isEntitledProductItemContext,
            ICallToActionViewModel callToActionViewModel)
			: base(model, isEntitledProductItemContext, textTranslator, callToActionViewModel)
		{
			RenderingParametersContext = renderingParametersContext;
		}

		public string Url => GlassModel._Url;

		public IEnumerable<ILinkable> ListableTopics
				=>
						GlassModel.Taxonomies?.Take(3)
								.Select(x => new LinkableModel { LinkableText = x.Item_Name, LinkableUrl = SearchTaxonomyUtil.GetSearchUrl(x) });

		public bool DisplayImage => Options.Show_Image && !string.IsNullOrEmpty(Image?.ImageUrl);

		public IFeatured_Article_Options Options => RenderingParametersContext.GetParameters<IFeatured_Article_Options>();
	}
}