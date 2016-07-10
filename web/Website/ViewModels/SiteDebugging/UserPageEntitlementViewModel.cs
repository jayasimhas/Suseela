using Informa.Library.Presentation;
using Informa.Library.Threading;
using Informa.Library.User.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Jabberwocky.Autofac.Attributes;
using System.Linq;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService]
	public class UserPageEntitlementViewModel : ThreadSafe<IEntitled_Product>, IUserPageEntitlementViewModel
	{
		protected readonly IRenderingItemContext RenderingItemContext;
		protected readonly IEntitledProductContext EntitledProductContext;
		protected readonly IEntitlementProductFactory EntitledProductFactory;
		protected readonly IEntitlementAccessContext EntitlementAccessContext;

		public UserPageEntitlementViewModel(
			IRenderingItemContext renderingItemContext,
			IEntitledProductContext entitledProductContext,
			IEntitlementProductFactory entitledProductFactory,
			IEntitlementAccessContext entitlementAccessContext)
		{
			RenderingItemContext = renderingItemContext;
			EntitledProductContext = entitledProductContext;
			EntitledProductFactory = entitledProductFactory;
			EntitlementAccessContext = entitlementAccessContext;
		}

		public IEntitled_Product EntitledProductItem => SafeObject;
		public bool IsValidPage => EntitledProductItem != null && EntitledProductItem._BaseTemplates.Contains(IEntitled_ProductConstants.TemplateId.Guid);
		public bool IsEntitled => EntitledProductContext.IsEntitled(EntitledProduct);
		public IEntitledProduct EntitledProduct => EntitledProductFactory.Create(EntitledProductItem);
		public IEntitlementAccess EntitlementAccess => EntitlementAccessContext.Find(EntitledProduct);
		protected override IEntitled_Product UnsafeObject => RenderingItemContext.Get<IEntitled_Product>();
	}
}