using Informa.Library.Purchase;
using Informa.Library.Purchase.User;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Collections.Generic;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class ArticlePurchasesViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IUserArticlePurchaseItemsContext UserArticlePurchaseItemsContext;

		public ArticlePurchasesViewModel(
			IAuthenticatedUserContext userContext,
			IUserArticlePurchaseItemsContext userArticlePurchaseItemsContext)
		{
			UserContext = userContext;
			UserArticlePurchaseItemsContext = userArticlePurchaseItemsContext;
		}

		public string Title => "Purchases";
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public string PublicationHeading => "Publication";
		public string TitleHeading => "Title";
		public string ExpirationHeading => "Expiration Date";
		public IEnumerable<IArticlePurchaseItem> ArticlePurchases => UserArticlePurchaseItemsContext.ArticlePurchaseItems;
	}
}