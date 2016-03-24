using Informa.Library.User.Entitlement;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService(LifetimeScope.Default)]
	public class UserEntitlementsViewModel : IUserEntitlementsViewModel
	{
		protected readonly IUserEntitlementsContext UserEntitlementsContext;

		public UserEntitlementsViewModel(
			IUserEntitlementsContext userEntitlementsContext)
		{
			UserEntitlementsContext = userEntitlementsContext;
		}

		public IEnumerable<IEntitlement> Entitlements => UserEntitlementsContext.Entitlements;
	}
}