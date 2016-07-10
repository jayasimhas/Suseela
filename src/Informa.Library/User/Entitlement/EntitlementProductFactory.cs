using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
	[AutowireService]
	public class EntitlementProductFactory : IEntitlementProductFactory
	{
		protected readonly IEntitledProductItemCodeFactory ProductCodeFactory;

		public EntitlementProductFactory(
			IEntitledProductItemCodeFactory productCodeFactory)
		{
			ProductCodeFactory = productCodeFactory;
		}

		public IEntitledProduct Create(IEntitled_Product item)
		{
			var productCode = ProductCodeFactory.Create(item);

			return new EntitledProduct
			{
				DocumentId = item.Article_Number,
				IsFree = item.Free,
				ProductCode = productCode,
				PublishedOn = item.Actual_Publish_Date
			};
		}
	}
}
