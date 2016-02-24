using Informa.Library.Presentation;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.General_Content;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
	public class PromotionalViewModel : GlassViewModel<IContent>
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;

		public PromotionalViewModel(
			IRenderingParametersContext renderingParametersContext)
		{
			RenderingParametersContext = renderingParametersContext;
		}

		public bool DisplayImage => Options.Show_Image;

		public IPromotional_Options Options => RenderingParametersContext.GetParameters<IPromotional_Options>();
	}
}