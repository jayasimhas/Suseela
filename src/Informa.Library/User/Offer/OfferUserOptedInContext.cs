using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Offer
{
	[AutowireService(LifetimeScope.Default)]
	public class OfferUserOptedInContext : IOfferUserOptedInContext
	{
		private const string sessionKey = nameof(OfferUserOptedInContext);

		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;
		protected readonly IOfferUserOptedIn OfferOptedIn;

		public OfferUserOptedInContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IOfferUserOptedIn offerOptedIn)
		{
			UserContext = userContext;
			UserSession = userSession;
			OfferOptedIn = offerOptedIn;
		}

		public bool OptedIn
		{
			get
			{
				if (!UserContext.IsAuthenticated)
				{
					return false;
				}

				var optedInSession = UserSession.Get<bool>(sessionKey);

				if (optedInSession.HasValue)
				{
					return optedInSession.Value;
				}

				var optedIn = OptedIn = OfferOptedIn.OptedIn(UserContext.User?.Username);

				return optedIn;
			}
			set
			{
				UserSession.Set(sessionKey, value);
			}
		}

		public void Clear()
		{
			UserSession.Clear(sessionKey);
		}
	}
}
