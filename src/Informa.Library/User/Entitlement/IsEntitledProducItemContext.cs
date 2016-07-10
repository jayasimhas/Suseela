using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.User.Entitlement
{
	[AutowireService]
	public class IsEntitledProducItemContext : IIsEntitledProducItemContext
	{
		protected readonly IEntitledProductContext IsEntitledProductContext;
		protected readonly IEntitlementProductFactory EntitledProductFactory;

		public IsEntitledProducItemContext(
			IEntitledProductContext isEntitledProductContext,
			IEntitlementProductFactory entitledProductFactory)
		{
			IsEntitledProductContext = isEntitledProductContext;
			EntitledProductFactory = entitledProductFactory;
		}

		public bool IsEntitled(IEntitled_Product item)
		{
			if (item == null)
			{
				return false;
			}

			var entitledProduct = EntitledProductFactory.Create(item);

			return IsEntitledProductContext.IsEntitled(entitledProduct);
		}
	}
}
