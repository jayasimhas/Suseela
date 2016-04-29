using Informa.Library.User.Authentication;
using Informa.Library.Utilities.References;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class SearchWrapperViewModel : GlassViewModel<IGlassBase>
	{
		public SearchWrapperViewModel(IItemReferences itemReferences, IAuthenticatedUserContext userContext)
		{
			ItemReferences = itemReferences;
			IsAuthenticated = userContext.IsAuthenticated;
		}

		public IItemReferences ItemReferences { get; set; }
		public bool IsAuthenticated { get; set; }
	}
}