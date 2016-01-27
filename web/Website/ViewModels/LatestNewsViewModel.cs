using Glass.Mapper.Sc;
using Informa.Library.Presentation;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels
{
	public class LatestNewsViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IRenderingParametersContext RenderingParametersContext;
		protected readonly ISitecoreContext SitecoreContext;

		public LatestNewsViewModel(
			IRenderingParametersContext renderingParametersContext,
			ISitecoreContext sitecoreContext)
		{
			RenderingParametersContext = renderingParametersContext;
			SitecoreContext = sitecoreContext;
		}

		public IEnumerable<string> Topics => new List<string> { { TestData.TestData.GetRandomTopic().LinkableText }, { TestData.TestData.GetRandomTopic().LinkableText } };

		public IEnumerable<IListable> News => Enumerable.Range(1, 6).Select(i => TestData.TestData.ListableModel(1, 0, false));

		public int ArticlesToDisplay
		{
			get
			{
				var optionItem = SitecoreContext.GetItem<INumber_Option>(Parameters.Number_To_Display);

				return optionItem == null ? 6 : optionItem.Value;
			}
		}

		public bool DisplayTitle => Parameters.Display_Title;

		public ILatest_News_Options Parameters => RenderingParametersContext.GetParameters<ILatest_News_Options>();
	}
}