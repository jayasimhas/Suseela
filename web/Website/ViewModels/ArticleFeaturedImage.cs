using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
    public class ArticleFeaturedImage : IFeaturedImage
    {
        private IArticle _glassModel;
        public ArticleFeaturedImage(IArticle glassModel)
        {
            _glassModel = glassModel;
        }

        public string ImageUrl => _glassModel?.Featured_Image_16_9?.Src ?? string.Empty;
        public string ImageCaption => _glassModel?.Featured_Image_Caption ?? string.Empty;
        public string ImageSource => _glassModel?.Featured_Image_Source ?? string.Empty;
	    public string ImageAltText => _glassModel?.Featured_Image_16_9?.Alt ?? string.Empty;
	}
}