using Informa.Library.Purchase;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class ArticlePurchasesViewModel
	{
		public ArticlePurchasesViewModel(
			)
		{

		}

		public bool IsAuthenticated => true;
		public string PublicationHeading => "Publication";
		public string TitleHeading => "Title";
		public string ExpirationHeading => "Expiration Date";
		public IEnumerable<IArticlePurchase> ArticlePurchases => Enumerable.Empty<IArticlePurchase>();
	}
}