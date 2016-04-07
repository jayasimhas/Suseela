using Informa.Library.Newsletter;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Newsletter
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class NewsletterUserOptInFactory : INewsletterUserOptInFactory
	{
		public INewsletterUserOptIn Create(NewsletterType newsletterOptInType, bool optIn)
		{
			return new NewsletterUserOptIn
			{
				OptIn = optIn,
				NewsletterType = newsletterOptInType
			};
		}
	}
}
