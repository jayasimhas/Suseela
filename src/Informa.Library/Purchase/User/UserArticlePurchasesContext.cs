using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

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
				if (!UserContext.IsAuthenticated)
				{
					return Enumerable.Empty<IArticlePurchase>();
				}

				var session = UserSession.Get<IEnumerable<IArticlePurchase>>(sessionKey);

				if (session.HasValue)
				{
					return session.Value;
				}

				var subscriptions = ArticlesPurchases = FindArticlePurchases.Find(UserContext.User?.Username ?? string.Empty);

				return subscriptions;
			}
			set
			{
				UserSession.Set(sessionKey, value);
			}
		}
	}
}
