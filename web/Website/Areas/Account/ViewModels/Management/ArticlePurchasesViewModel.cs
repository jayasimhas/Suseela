using Informa.Library.Purchase;
using Informa.Library.Purchase.User;
using Informa.Library.User.Authentication;
using System.Collections.Generic;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class ArticlePurchasesViewModel
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

		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public string PublicationHeading => "Publication";
		public string TitleHeading => "Title";
		public string ExpirationHeading => "Expiration Date";
		public IEnumerable<IArticlePurchaseItem> ArticlePurchases => UserArticlePurchaseItemsContext.ArticlePurchaseItems;
	}
}