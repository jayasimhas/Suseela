using Informa.Library.Utilities.References;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class SearchWrapperViewModel : GlassViewModel<IGlassBase>
	{
		public SearchWrapperViewModel(IItemReferences itemReferences)
		{
			ItemReferences = itemReferences;
		}

		public IItemReferences ItemReferences { get; set; }
	}
}