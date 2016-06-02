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
				if (!AuthenticatedUserContext.IsAuthenticated)
				{
					return DefaultEntitlementsFactory.Create();
				}

				var entitlementsSession = UserSession.Get<IEnumerable<IEntitlement>>(EntitlementSessionKey);

				if (entitlementsSession.HasValue)
				{
					return entitlementsSession.Value;
				}

				var entitlements = GetUserEntitlements.GetEntitlements(AuthenticatedUserContext.User?.Username, UserIpAddressContext.IpAddress.ToString());

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

		public EntitledAccessLevel AccessLevel => EntitledAccessLevel.Individual;
	}
}
