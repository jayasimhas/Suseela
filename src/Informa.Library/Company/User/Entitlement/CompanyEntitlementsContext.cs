using System.Collections.Generic;
using System.Linq;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.User;
using Informa.Library.User.Entitlement;

namespace Informa.Library.Company.User.Entitlement
{
	[AutowireService(LifetimeScope.Default)]
	public class CompanyEntitlementsContext : ICompanyEntitlementsContext
	{
		protected readonly IUserCompanyContext CompanyContext;
		protected readonly IUserIpAddressContext UserIpAddressContext;
		protected readonly IUserSession UserSession;
		protected readonly IGetIPEntitlements GetIpEntitlements;
		protected readonly IDefaultEntitlementsFactory DefaultEntitlementsFactory;

		public CompanyEntitlementsContext(
			IUserCompanyContext companyContext,
			IUserIpAddressContext userIpAddressContext,
			IUserSession userSession,
			IGetIPEntitlements getIpEntitlements,
			IDefaultEntitlementsFactory defaultEntitlementsFactory)
		{
			CompanyContext = companyContext;
			UserIpAddressContext = userIpAddressContext;
			UserSession = userSession;
			GetIpEntitlements = getIpEntitlements;
			DefaultEntitlementsFactory = defaultEntitlementsFactory;
		}

		private const string EntitlementSessionKeyPrefix = nameof(CompanyEntitlementsContext);

		public string EntitlementSessionKey => $"{EntitlementSessionKeyPrefix}{UserIpAddressContext.IpAddress}";

		public IEnumerable<IEntitlement> Entitlements
		{
			get
			{
				var entitlementsSession = UserSession.Get<IEnumerable<IEntitlement>>(EntitlementSessionKey);

				if (entitlementsSession.HasValue)
				{
					return entitlementsSession.Value;
				}

				var entitlements = new List<IEntitlement>();
				entitlements.AddRange(GetIpEntitlements.GetEntitlements(UserIpAddressContext.IpAddress));

				Entitlements = entitlements.Any() ? entitlements : DefaultEntitlementsFactory.Create();

				return entitlements;
			}
			set { UserSession.Set(EntitlementSessionKey, value); }
		}

		public void RefreshEntitlements()
		{
			UserSession.Clear(EntitlementSessionKey);
		}

		public EntitledAccessLevel AccessLevel
		{
			get
			{
				if (CompanyContext.Company == null)
				{
					return EntitledAccessLevel.None;
				}

				switch (CompanyContext.Company.Type)
				{
					case CompanyType.SiteLicenseIP:
						return EntitledAccessLevel.Site;
					case CompanyType.TransparentIP:
						return EntitledAccessLevel.TransparentIP;
					default:
						return EntitledAccessLevel.None;
				}
			}
		}
	}
}