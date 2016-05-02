using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.Purchase.User
{
	[AutowireService(LifetimeScope.PerScope)]
	public class UserArticlePurchasesContext : IUserArticlePurchasesContext
	{
		private const string sessionKey = nameof(UserArticlePurchasesContext);

		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;
		protected readonly IFindUserArticlePurchases FindArticlePurchases;

		public UserArticlePurchasesContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IFindUserArticlePurchases findArticlePurchases)
		{
			UserContext = userContext;
			UserSession = userSession;
			FindArticlePurchases = findArticlePurchases;
		}

		public IEnumerable<IArticlePurchase> ArticlesPurchases
		{
			get
			{
				var subscriptionSession = UserSession.Get<IEnumerable<IArticlePurchase>>(sessionKey);

				if (subscriptionSession.HasValue)
				{
					return subscriptionSession.Value;
				}

				var subscriptions = ArticlesPurchases = FindArticlePurchases.Find(UserContext.User.Username);

				return subscriptions;
			}
			set
			{
				UserSession.Set(sessionKey, value);
			}
		}
	}
}
