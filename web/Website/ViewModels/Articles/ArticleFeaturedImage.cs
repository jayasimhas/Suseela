using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels.Articles
{
	public class ArticleFeaturedImage : FeaturedImage
	{
		private IArticle _glassModel;
		public ArticleFeaturedImage(IArticle glassModel)
		{
			_glassModel = glassModel;
		}

		public override string ImageUrl => _glassModel?.Featured_Image_16_9?.Src ?? string.Empty;
		public override string ImageCaption => _glassModel?.Featured_Image_Caption ?? string.Empty;
		public override string ImageSource => _glassModel?.Featured_Image_Source ?? string.Empty;
		public override string ImageAltText => _glassModel?.Featured_Image_16_9?.Alt ?? string.Empty;
	}
}