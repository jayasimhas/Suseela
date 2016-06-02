using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.PerScope)]
	public class NewsletterUserOptInsContext : INewsletterUserOptInsContext
	{
		private const string sessionKey = nameof(NewsletterUserOptInsContext);

		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;
		protected readonly IFindNewsletterUserOptIns NewsletterOptIn;

		public NewsletterUserOptInsContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IFindNewsletterUserOptIns newsletterOptIn)
		{
			NewsletterOptIn = newsletterOptIn;
			UserSession = userSession;
			UserContext = userContext;
		}

		public IEnumerable<INewsletterUserOptIn> OptIns
		{
			get
			{
				if (!UserContext.IsAuthenticated)
				{
					return Enumerable.Empty<INewsletterUserOptIn>();
				}

				var optInsSession = UserSession.Get<IEnumerable<INewsletterUserOptIn>>(sessionKey);

				if (optInsSession.HasValue)
				{
					return optInsSession.Value;
				}

				var optIns = OptIns = UserContext.IsAuthenticated ? NewsletterOptIn.Find(UserContext.User?.Username) : Enumerable.Empty<INewsletterUserOptIn>();

				return optIns;
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
