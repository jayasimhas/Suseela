using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Profile
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class NewsletterUserOptInsContext : INewsletterUserOptInsContext
	{
		private const string sessionKey = nameof(NewsletterUserOptInsContext);

		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IAuthenticatedUserSession UserSession;
		protected readonly IFindNewsletterUserOptIn NewsletterOptIn;

		public NewsletterUserOptInsContext(
			IAuthenticatedUserContext userContext,
			IAuthenticatedUserSession userSession,
			IFindNewsletterUserOptIn newsletterOptIn)
		{
			NewsletterOptIn = newsletterOptIn;
			UserContext = userContext;
		}

		public IEnumerable<INewsletterUserOptIn> OptIns
		{
			get
			{
				var optInsSession = UserSession.Get<IEnumerable<INewsletterUserOptIn>>(sessionKey);

				if (optInsSession.HasValue)
				{
					return optInsSession.Value;
				}

				var optIns = OptIns = UserContext.IsAuthenticated ? NewsletterOptIn.Find(UserContext.User.Username) : Enumerable.Empty<INewsletterUserOptIn>();

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
