using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using Informa.Library.User.Authentication;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class AuthenticatedUserEntitlementsContext : IAuthenticatedUserEntitlementsContext
	{
		private const string EntitlementSessionKey = nameof(AuthenticatedUserEntitlementsContext);

		protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
		protected readonly IAuthenticatedUserSession UserSession;
		protected readonly IGetUserEntitlements GetUserEntitlements;
		protected readonly IUserIpAddressContext UserIpAddressContext;
		protected readonly IDefaultEntitlementsFactory DefaultEntitlementsFactory;

		public AuthenticatedUserEntitlementsContext(
			IAuthenticatedUserContext authenticatedUserContext,
			IAuthenticatedUserSession userSession,
			IGetUserEntitlements getUserEntitlements,
			IUserIpAddressContext userIpAddressContext,
			IDefaultEntitlementsFactory defaultEntitlementsFactory)
		{
			AuthenticatedUserContext = authenticatedUserContext;
			UserSession = userSession;
			GetUserEntitlements = getUserEntitlements;
			UserIpAddressContext = userIpAddressContext;
			DefaultEntitlementsFactory = defaultEntitlementsFactory;
		}

		public IEnumerable<IEntitlement> Entitlements
		{
			get
			{
				var entitlementsSession = UserSession.Get<IList<IEntitlement>>(EntitlementSessionKey);
				var entitlements = entitlementsSession.HasValue ? entitlementsSession.Value : new List<IEntitlement>();

				if (entitlements.Any())
				{
					return entitlements;
				}

				entitlements = GetUserEntitlements.GetEntitlements(AuthenticatedUserContext.User.Username, UserIpAddressContext.IpAddress.ToString());

				if (!entitlements.Any())
				{
					entitlements = DefaultEntitlementsFactory.Create().ToList();
				}

				Entitlements = entitlements;

				return entitlements;
			}
			set
			{
				UserSession.Set(EntitlementSessionKey, value);
			}
		}

		public void RefreshEntitlements()
		{
			UserSession.Clear(EntitlementSessionKey);
		}

		public EntitledAccessLevel GetProductAccessLevel(string productCode)
		{
			return Entitlements.Any(e => e.ProductCode == productCode) ? EntitledAccessLevel.Individual : EntitledAccessLevel.UnEntitled;
		}
	}
}
