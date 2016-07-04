using Informa.Library.Globalization;
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
		protected readonly ITextTranslator TextTranslator;

		public ArticlePurchasesViewModel(
			IAuthenticatedUserContext userContext,
			IUserArticlePurchaseItemsContext userArticlePurchaseItemsContext,
			ITextTranslator textTranslator)
		{
			UserContext = userContext;
			UserArticlePurchaseItemsContext = userArticlePurchaseItemsContext;
			TextTranslator = textTranslator;
		}

		public string Title => TextTranslator.Translate("ArticlePurchases.Title");
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public string PublicationHeading => TextTranslator.Translate("ArticlePurchases.PublicationHeading");
		public string TitleHeading => TextTranslator.Translate("ArticlePurchases.TitleHeading");
		public string ExpirationHeading => TextTranslator.Translate("ArticlePurchases.ExpirationHeading");
		public IEnumerable<IArticlePurchaseItem> ArticlePurchases => UserArticlePurchaseItemsContext.ArticlePurchaseItems;
	}
}