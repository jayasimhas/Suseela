using Informa.Library.Presentation;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.General_Content;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.ContentListing
{
	public class ContentListingViewModel : GlassViewModel<IContent>
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;

		public ContentListingViewModel(
			IRenderingParametersContext renderingParametersContext)
		{
			RenderingParametersContext = renderingParametersContext;
		}

		public bool DisplayImage => Options.Show_Image;

		public IContent_Listing_Options Options => RenderingParametersContext.GetParameters<IContent_Listing_Options>();
	}
}